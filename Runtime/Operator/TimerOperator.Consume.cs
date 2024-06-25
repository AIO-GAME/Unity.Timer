#region

using System;
using System.Collections.Generic;

#endregion

namespace AIO
{
    /// <summary>
    /// 消耗型定时器
    /// </summary>
    internal class TimerOperatorConsume : TimerOperator
    {
        /// <summary>
        /// 完成事件
        /// </summary>
        private readonly Action<List<ITimerExecutor>> EventDone;

        /// <summary>
        /// 循环事件
        /// </summary>
        private readonly Action<List<ITimerExecutor>> EventLoop;

        /// <summary>
        /// 进化事件
        /// </summary>
        private readonly Action<int, List<ITimerExecutor>> EventEvolution;

        public TimerOperatorConsume(
            ITimerOperator                    timerOperator,
            Action<List<ITimerExecutor>>      eventDone,
            Action<List<ITimerExecutor>>      eventLoop,
            Action<int, List<ITimerExecutor>> eventEvolution)
        {
            Index    = timerOperator.Index;
            Slot     = timerOperator.Slot;
            SlotUnit = timerOperator.SlotUnit;
            AllCount = timerOperator.AllCount;
            MaxCount = timerOperator.MaxCount;
            Unit     = timerOperator.Unit;

            TimersCache = timerOperator.TimersCache;
            Timers      = timerOperator.Timers;

            EventDone      = eventDone;
            EventLoop      = eventLoop;
            EventEvolution = eventEvolution;
        }

        public override int BottomUpdate(long nowTime)
        {
            var DoneList = Pool.List<ITimerExecutor>(); // 用于存储已经完成的任务
            var LoopList = Pool.List<ITimerExecutor>(); // 用于存储需要循环的任务

            var finishNumber = 0;
            lock (Timers)
            {
                while (Timers.Count > 0 && Timers.First != null) //判断当前需要移除哪些任务
                {
                    var executor = Timers.First.Value;
                    if (executor.EndTime <= nowTime)
                    {
                        finishNumber++;
                        if (executor.UpdateLoop()) LoopList.Add(executor);
                        DoneList.Add(executor);
                        Timers.RemoveFirst();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            AllCount -= finishNumber;
            if (LoopList.Count > 0) EventLoop.Invoke(LoopList);
            else LoopList.Free();

            if (DoneList.Count > 0) EventDone.Invoke(DoneList);
            else DoneList.Free();

            return finishNumber;
        }

        public override void OtherUpdate(long nowTime)
        {
            var EvolutionList = Pool.List<ITimerExecutor>(); // 用于存储已经完成的任务
            lock (Timers)
            {
                while (Timers.Count > 0 && Timers.First != null) //判断当前需要移除哪些任务
                {
                    var executor = Timers.First.Value;
                    if (executor.EndTime - nowTime < Unit) //如果发现当前剩余时间不满足当前层级时间总量 则移动到下一次层级
                    {
                        EvolutionList.Add(executor);
                        Timers.RemoveFirst();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (EvolutionList.Count > 0)
            {
                AllCount -= EvolutionList.Count;
                EventEvolution?.Invoke(Index - 1, EvolutionList);
            }
            else
            {
                EvolutionList.Free();
            }
        }
    }
}
