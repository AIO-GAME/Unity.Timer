#region

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

#endregion

namespace AIO
{
    /// <summary>
    /// 辅助定时器
    /// </summary>
    internal sealed class TimerContainerConsume : TimerContainer
    {
        public TimerContainerConsume(long unit, long counter, List<ITimerOperator> operators)
        {
            Unit    = unit;
            Counter = counter;

            var isSort = true;
            lock (operators)
            {
                for (var i = operators.Count - 1; i >= 0; i--)
                {
                    if (isSort && operators[i].AllCount <= 0) continue;
                    isSort = false;
                    var op = new TimerOperatorConsume(operators[i], DoneCallBack, LoopCallBack, EvolutionCallBack);
                    RemainNum += op.AllCount; //计算当前定时器数量
                    List.Add(op);
                }
            }

            if (List.Count <= 0) return;
            List.Sort((a, b) =>
            {
                if (a.Index < b.Index) return -1;
                return a.Index > b.Index ? 1 : 0;
            });

            TotalNum = RemainNum;
        }

        /// <summary>
        /// 当前辅助定时器总数量
        /// </summary>
        private int TotalNum { get; }

        protected override void Update()
        {
            TaskHandleToken.ThrowIfCancellationRequested();

            foreach (var operators in List) operators.TimersUpdate(); //更新缓存与执行容器

            long nowMilliseconds;
            try
            {
                while (TimerSystem.SWITCH && RemainNum > 0)
                {
                    nowMilliseconds = Watch.ElapsedMilliseconds;

                    if (nowMilliseconds < Unit)
                    {
                        Thread.Sleep(10);
                        continue; // 更新间隔
                    }

                    Watch.Restart();
                    Counter         += nowMilliseconds;
                    UpdateCacheTime += nowMilliseconds;
                    if (UpdateCacheTime > TimerSystem.UPDATELISTTIME)
                    {
                        UpdateCacheTime = 0; // 重置缓存更新时间
                        foreach (var item in List) item.TimersUpdate();
                    }

                    List[0].SlotUpdate(Unit);
                    RemainNum = RemainNum - List[0].BottomUpdate(Counter);
                    if (RemainNum <= 0)
                    {
                        RemainNum = 0; //重新计算剩余数量 保证异步线程修改 出现数据丢失
                        foreach (var item in List) RemainNum = RemainNum + item.AllCount;
                        if (RemainNum <= 0) break;
                    }

                    if (List[0].Slot < List[0].SlotUnit) continue;

                    List[0].SlotReset();
                    for (var i = 1; i < List.Count; i++)
                    {
                        List[i].SlotUpdate(List[i - 1].Unit);
                        if (List[i].Slot >= List[i].SlotUnit)
                        {
                            List[i].OtherUpdate(Counter);
                            List[i].SlotReset();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
#if UNITY_EDITOR
                Debug.Log(
                    $"[辅助定时器:{ID}] [容器数量:{List.Count}] [状态:结束] 精度单位:{Unit} 当前时间:{Counter} 任务总数量:{TotalNum} 完成任务数量:{TotalNum - RemainNum} 剩余任务数量:{RemainNum}");
#endif
            }
#if UNITY_EDITOR
            catch (Exception e)
#else
            catch (Exception)
#endif
            {
#if UNITY_EDITOR
                Debug.LogErrorFormat(
                    $"[辅助定时器:{ID}] [容器数量:{List.Count}] [状态:异常] 精度单位:{Unit} 当前时间:{Counter} 任务总数量:{TotalNum} 完成任务数量:{TotalNum - RemainNum} 异常信息:{e}");
#endif
            }
            finally
            {
                Dispose();
            }
        }

        private void DoneCallBack(List<ITimerExecutor> list)
        {
            lock (list)
            {
                foreach (var item in list) Runner.PushUpdate(item.Execute);
                list.Free();
            }
        }

        private void LoopCallBack(List<ITimerExecutor> list)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var executor = list[i];
                if (executor.OperatorIndex >= List.Count) continue;
                switch (executor.OperatorIndex)
                {
                    case 0:
                        List[0].AddTimerSource(executor);
                        break;
                    default:
                        List[executor.OperatorIndex].AddTimerCache(executor);
                        break;
                }

                list.RemoveAt(i);
            }

            TimerSystem.AddLoop(list);
        }

        private void EvolutionCallBack(int Index, List<ITimerExecutor> list)
        {
            List[Index].AddTimerSource(list);
        }

        public override void Dispose()
        {
            TimerSystem.DisposeContainer(this);
            base.Dispose();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(
                $"[{GetType().Name} ID:{ID}] [容器数量:{List.Count}] 精度单位:{Unit} 当前时间:{Counter} 任务总数量:{TotalNum} 完成任务数量:{TotalNum - RemainNum} 剩余任务数量:{RemainNum}");
            foreach (var item in List) builder.AppendLine(item.ToString()).AppendLine();
            return builder.ToString();
        }
    }
}