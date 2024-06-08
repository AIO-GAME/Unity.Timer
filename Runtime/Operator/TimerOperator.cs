#region

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#endregion

namespace AIO
{
    /// <summary>
    /// 消耗型定时器
    /// </summary>
    public abstract class TimerOperator : ITimerOperator
    {
        protected TimerOperator()
        {
            TimersCache = Pool.List<ITimerExecutor>();
            Timers      = Pool.LinkedList<ITimerExecutor>();
            Slot        = 0;
            MaxCount    = 2048;
        }

        public TimerOperator(int index, long unit, long slotUnit, int maxCount = 2048)
        {
            TimersCache = Pool.List<ITimerExecutor>();
            Timers      = Pool.LinkedList<ITimerExecutor>();
            Slot        = 0;
            MaxCount    = maxCount;

            Index    = index;
            Unit     = unit * slotUnit;
            SlotUnit = slotUnit;
        }

        /// <summary>
        /// 计算当前瞬间 定时器全部数量
        /// </summary>
        public int AllCount { get; internal set; }

        #region ITimerOperator Members

        public int Index { get; protected set; }

        public long Unit { get; protected set; }

        public long SlotUnit { get; protected set; }

        public LinkedList<ITimerExecutor> Timers { get; protected set; }

        public List<ITimerExecutor> TimersCache { get; protected set; }

        int ITimerOperator.AllCount
        {
            get => AllCount;
            set => AllCount = value;
        }

        public int MaxCount { get; protected set; }

        public long Slot { get; protected set; }

        public void Dispose()
        {
            TimersCache.Free();
            Timers.Free();
        }

        public sealed override string ToString()
        {
#if UNITY_EDITOR
            var @string = new StringBuilder();
            @string.Append("当前毫秒:").Append(TimerSystem.Counter).Append("ms").AppendLine();
            @string.Append("定时器序号:").Append(Index).AppendLine();
            @string.Append("计时单位:").Append(Unit).Append("ms").AppendLine();
            @string.Append("当前时间:").Append(Slot).AppendLine();
            @string.Append("队列数量:").Append(Timers.Count);
            if (Timers.Count <= 0) return @string.ToString();

            @string.Append("\n队列信息:\n[");
            lock (Timers)
            {
                foreach (var item in Timers)
                {
                    @string.AppendLine().Append("定时单位 =").Append(item.Duration).Append("ms").Append(' ');
                    @string.Append("创建时间 =").Append(item.CreateTime).Append("ms").Append(' ');
                    @string.Append("结束时间 =").Append(item.EndTime).Append("ms").Append(' ');
                }
            }


            return @string.AppendLine("\n]").ToString();
#else
            return string.Empty;
#endif
        }

        public virtual void AddTimerSource(ITimerExecutor executor)
        {
            AllCount += 1;
            TimersUpdate(executor);
        }

        public virtual void AddTimerSource(List<ITimerExecutor> executors)
        {
            if (executors.Count == 0) return;
            AllCount += executors.Count;
            TimersUpdate(executors);
        }

        /// <summary>
        /// 添加任务执行器 进入缓存
        /// </summary>
        public virtual void AddTimerCache(ITimerExecutor executor)
        {
            lock (TimersCache)
            {
                TimersCache.Add(executor);
                if (TimersCache.Count >= MaxCount)
                {
                    TimersUpdate(TimersCache); //如果里当前数量接近容量值 则立马添加到Timers中
                    TimersCache = Pool.List<ITimerExecutor>();
                }
            }

            AllCount += 1;
        }

        public virtual void AddTimerCache(params ITimerExecutor[] executors)
        {
            if (executors.Length == 0) return;
            lock (TimersCache)
            {
                for (var i = 0; i < executors.Length; i++) TimersCache.Add(executors[i]);
                if (TimersCache.Count >= MaxCount)
                {
                    TimersUpdate(TimersCache);
                    TimersCache = Pool.List<ITimerExecutor>();
                }
            }

            AllCount += executors.Length;
        }

        public void SlotUpdate(long UpdateSlot = 1)
        {
            Slot += UpdateSlot;
        }

        public void SlotReset()
        {
            Slot = 0;
        }

        public void ReceiveFromData(ITimerOperator @operator)
        {
            Index    = @operator.Index;
            Slot     = @operator.Slot;
            SlotUnit = @operator.SlotUnit;
            AllCount = @operator.AllCount;
            MaxCount = @operator.MaxCount;
            Unit     = @operator.Unit;

            TimersCache = @operator.TimersCache;
            Timers      = @operator.Timers;
        }

        public void TimersUpdate()
        {
            if (TimersCache.Count == 0) return;
            lock (TimersCache)
            {
                TimersUpdate(TimersCache);
                TimersCache = Pool.List<ITimerExecutor>();
            }
        }

        public abstract int BottomUpdate(long nowTime);

        public abstract void OtherUpdate(long nowTime);

        #endregion

        /// <summary>
        /// 获取新的定时器容器
        /// </summary>
        internal static ICollection<T> NewTimerOperator<T>()
        where T : TimerOperator, new()
        {
            var List = Pool.List<T>();
            foreach (var (allSlot, Uint, Slot) in TimerSystem.TimingUnits)
            {
                var operators = Activator.CreateInstance<T>();
                operators.Index    = List.Count;
                operators.Unit     = Uint * Slot;
                operators.SlotUnit = Slot;
                operators.MaxCount = 2048;
                operators.Slot     = 0;
                List.Add(operators);
            }

            return List;
        }

        /// <summary>
        /// 更新链表
        /// </summary>
        protected virtual void TimersUpdate(ITimerExecutor executor)
        {
            //当前为单项遍历 只遍历一次 准备优化为 双向遍历 只遍历一次
            lock (Timers)
            {
                var nowNode = Timers.First;
                while (true)
                {
                    if (nowNode == null)
                    {
                        Timers.AddLast(executor);
                        return;
                    }

                    if (executor.EndTime < nowNode.Value.EndTime)
                    {
                        Timers.AddBefore(nowNode, executor);
                        return;
                    }

                    nowNode = nowNode.Next;
                }
            }
        }

        /// <summary>
        /// 更新链表
        /// </summary>
        protected virtual void TimersUpdate(List<ITimerExecutor> executors)
        {
            try
            {
                executors.Sort((a, b) =>
                {
                    if (a.EndTime > b.EndTime) return 1;
                    if (a.EndTime < b.EndTime) return -1;
                    return 0;
                });

                //当前为单项遍历 只遍历一次 准备优化为 双向遍历 只遍历一次
                lock (Timers)
                {
                    var nowNode = Timers.First;
                    var lindex = 0;
                    while (true)
                    {
                        if (executors.Count <= lindex) break;

                        if (nowNode == null)
                        {
                            //1 如果当前节点 值为Null 说明整条节点没有合适的了 所以可以直接赋值 退出循环
                            while (executors.Count > lindex) Timers.AddLast(executors[lindex++]);
                            break; //因为当前已经遍历到最后一个了 所以可以直接让之后的数据 依次接入链表
                        }

                        if (executors[lindex].EndTime < nowNode.Value.EndTime)
                        {
                            //2 如果待加入 结束时间 < 当前结束时间 那么说明 可以直接赋值 退出循环
                            nowNode = Timers.AddBefore(nowNode, executors[lindex++]);
                            continue;
                        }

                        nowNode = nowNode.Next;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                executors.Free();
            }
        }
    }
}