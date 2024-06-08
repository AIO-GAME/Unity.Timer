#region

using System;
using System.Collections.Generic;

#endregion

namespace AIO
{
    /// <summary>
    /// 消耗型定时器
    /// </summary>
    internal class TimerOperatorLoop : TimerOperator
    {
        private Action<List<ITimerExecutor>>      DoneEvent;
        private Action<int, List<ITimerExecutor>> EvolutionEvent;
        private Action<List<ITimerExecutor>>      LoopEvent;

        public TimerOperatorLoop(
            byte                              index,
            long                              unit,
            long                              slotUnit,
            Action<List<ITimerExecutor>>      doneEvent,
            Action<List<ITimerExecutor>>      loopEvent,
            Action<int, List<ITimerExecutor>> evolutionEvent,
            int                               maxCount = 2048
        ) : base(index, unit, slotUnit, maxCount)
        {
            DoneEvent      = doneEvent;
            LoopEvent      = loopEvent;
            EvolutionEvent = evolutionEvent;
        }

        public override int BottomUpdate(long nowTime)
        {
            var DoneList = Pool.List<ITimerExecutor>(); // 用于存储已经完成的任务
            var LoopList = Pool.List<ITimerExecutor>(); // 用于存储需要循环的任务

            var FinishNumber = 0;
            lock (Timers)
            {
                while (Timers.Count > 0 && Timers.First != null) //判断当前需要移除哪些任务
                {
                    var executor = Timers.First.Value;
                    if (executor.EndTime <= nowTime)
                    {
                        FinishNumber++;
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

            AllCount -= FinishNumber;
            if (DoneList.Count > 0)
            {
                DoneEvent.Invoke(DoneList);
                if (LoopList.Count > 0) LoopEvent.Invoke(LoopList);
                else LoopList.Free();
            }
            else
            {
                DoneList.Free();
                LoopList.Free();
            }

            return FinishNumber;
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
                EvolutionEvent.Invoke(Index - 1, EvolutionList);
            }
            else
            {
                EvolutionList.Free();
            }
        }
    }
}