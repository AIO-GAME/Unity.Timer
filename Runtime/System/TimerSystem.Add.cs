#region

using System.Collections.Generic;

#endregion

namespace AIO
{
    partial class TimerSystem
    {
        /// <summary>
        /// 加入指定分层定时器
        /// 加入时 默认TimerExe参数正确
        /// </summary>
        /// <param name="index">定时器层级</param>
        /// <param name="timer">执行器</param>
        internal static void UpdateSlot(int index, ITimerExecutor timer)
        {
            lock (MainList)
            {
                MainList[index].AddTimerSource(timer);
            }
        }

        /// <summary>
        /// 加入指定分层定时器
        /// 加入时 默认TimerExe参数正确
        /// </summary>
        /// <param name="index">定时器层级</param>
        /// <param name="timer">执行器</param>
        internal static void UpdateSlot(int index, List<ITimerExecutor> timer)
        {
            lock (MainList)
            {
                MainList[index].AddTimerSource(timer);
            }
        }

        #region Add

        private static void AddUpdate(ITimerExecutor timer)
        {
            for (byte i = 0; i < TimingUnits.Count - 1; i++)
            {
                //说明 当前 时间分级器单位 与 当前定时任务处理器 匹配
                //当I等于最后一个分级层数时
                if (timer.Duration > TimingUnits[i].Item1 && i != TimingUnits.Count - 1) continue;
                if (i == 0) timer.OperatorIndex = 0;
                else timer.OperatorIndex        = (byte)(i - 1);
                break;
            }

            if (TimerSystemSettings.EnableLoopThread && timer.Loop == -1)
            {
                LoopContainer.PushUpdate(timer);
                return;
            }

            if (RemainNum >= Capacity)
                lock (MainList)
                {
                    //如果大于容量 则单独开一个线程 清空当前主函数中 所有列表
                    var v = new TimerContainerConsume(Unit, Counter + Watch.ElapsedMilliseconds, MainList);
                    RegisterList();
                    v.Start();
                    RemainNum = RemainNum - Capacity; //因为线程计算问题 只能定时到一瞬间 因为数据的转移方式是全部转移 所以这里直接减去容量
                    lock (TaskList)
                    {
                        TaskList.Add(v);
                    }
                }

            AddLoop(timer);
            RemainNum = RemainNum + 1;
        }

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        internal static void AddLoop(ITimerExecutor timer)
        {
            lock (MainList)
            {
                switch (timer.OperatorIndex)
                {
                    case 0:
                        MainList[0].AddTimerSource(timer);
                        break;
                    default:
                        MainList[timer.OperatorIndex].AddTimerCache(timer);
                        break;
                }
            }
        }

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        internal static void AddLoop(List<ITimerExecutor> timers)
        {
            if (timers is null) return;

            if (timers.Count == 0)
            {
                timers.Free();
                return;
            }

            lock (MainList)
            {
                for (var i = 0; i < timers.Count; i++)
                    switch (timers[i].OperatorIndex)
                    {
                        case 0:
                            MainList[0].AddTimerSource(timers[i]);
                            break;
                        default:
                            MainList[timers[i].OperatorIndex].AddTimerCache(timers[i]);
                            break;
                    }

                timers.Free();
            }
        }

        #endregion
    }
}