#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace AIO
{
    /// <summary>
    /// 定时器 时间调度器 循环
    /// </summary>
    public class TimerContainerLoop : TimerContainer
    {
        public TimerContainerLoop(long unit)
        {
            Unit = unit;

            var tuples = TimerSystem.TimingUnits;
            for (byte i = 0; i < tuples.Count; i++)
                List.Add(new TimerOperatorLoop(i, tuples[i].Item2, tuples[i].Item3, DoneCallBack, PushUpdate, EvolutionCallBack));
        }

        protected override void Update()
        {
            TaskHandleToken.ThrowIfCancellationRequested();
            long nowMilliseconds;
            Counter = TimerSystem.Counter;
            try
            {
                while (TimerSystem.SWITCH)
                {
                    nowMilliseconds = Watch.ElapsedMilliseconds;
                    if (nowMilliseconds < Unit) continue; //更新间隔
                    Watch.Restart();
                    Counter         += nowMilliseconds;
                    UpdateCacheTime += nowMilliseconds;

                    if (UpdateCacheTime > TimerSystem.UPDATELISTTIME)
                    {
                        UpdateCacheTime = 0; // 重置缓存更新时间
                        foreach (var item in List) item.TimersUpdate();
                    }

                    List[0].SlotUpdate(Unit);
                    if (RemainNum <= 0)
                    {
                        if (List[0].Slot >= List[0].SlotUnit)
                        {
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
                    }
                    else
                    {
                        RemainNum = RemainNum - List[0].BottomUpdate(Counter);
                        if (RemainNum <= 0)
                        {
                            RemainNum = 0; //重新计算剩余数量 保证异步线程修改 出现数据丢失
                            foreach (var item in List) RemainNum = RemainNum + item.AllCount;
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
                }


#if UNITY_EDITOR
                Debug.Log(
                          $"[循环定时器:{ID}] [容器数量:{List.Count}] [状态:结束] 精度单位:{Unit} 当前时间:{Counter} 剩余任务数量:{RemainNum}");
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
                                     $"[循环定时器:{ID}] [容器数量:{List.Count}] [状态:异常] 精度单位:{Unit} 当前时间:{Counter} 剩余任务数量:{RemainNum} 异常信息:{e}");
#endif
            }
            finally
            {
                Dispose();
            }
        }

        private void DoneCallBack(List<ITimerExecutor> list)
        {
            Runner.StartTask(() =>
            {
                foreach (var item in list) item.Execute();
                list.Free();
            });
        }

        private void EvolutionCallBack(int Index, List<ITimerExecutor> list) { List[Index].AddTimerSource(list); }
    }
}
