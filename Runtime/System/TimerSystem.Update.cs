#region

using System;
using UnityEngine;

#endregion

namespace AIO
{
    partial class TimerSystem
    {
        /// <summary>
        /// 自旋 周期执行函数
        /// 时间驱动 从下往上 单位小的 达到条件 通知单位大的
        /// 层层通知 达到60秒通知1分钟 达到60分钟 通知1小时 达到24小时 通知1天 这样 可以分层 减轻最底层处理逻辑负担 提高计算效率
        ///
        /// 事件驱动 从上往下 单位事件大的检测 达到条件 移交给单位小一级的 层层移交 如果移交不下去了 则判定为过期任务 需要执行销毁
        /// </summary>
        static partial void Update()
        {
            TaskHandleToken.ThrowIfCancellationRequested();
            try
            {
#if UNITY_EDITOR
                Debug.Log("定时器线程启动");
#endif
                while (SWITCH)
                {
                    var updateCacheTime = Watch.ElapsedMilliseconds;
                    if (updateCacheTime < Unit) continue; //更新间隔
                    Watch.Restart();
                    Counter         += updateCacheTime;
                    UpdateCacheTime += updateCacheTime;

                    if (UpdateCacheTime > UPDATELISTTIME)
                    {
                        UpdateCacheTime = 0; // 重置缓存更新时间
                        for (var i = 0; i < MainList.Count; i++) MainList[i].TimersUpdate();
                    }

                    lock (MainList)
                    {
                        if (RemainNum <= 0) // 更新格子
                        {
                            MainList[0].SlotUpdate(Unit);
                            if (MainList[0].Slot >= MainList[0].SlotUnit)
                            {
                                MainList[0].SlotReset();
                                for (var i = 1; i < MainList.Count; i++)
                                {
                                    MainList[i].SlotUpdate(MainList[i - 1].Unit);
                                    if (MainList[i].Slot >= MainList[i].SlotUnit)
                                        MainList[i].SlotReset();
                                    else break;
                                }
                            }
                        }
                        else
                        {
                            MainList[0].SlotUpdate(Unit);
                            RemainNum = RemainNum - MainList[0].BottomUpdate(Counter);
                            if (RemainNum <= 0)
                            {
                                RemainNum = 0; //重新计算剩余数量 保证异步线程修改 出现数据丢失
                                foreach (var item in MainList) RemainNum = RemainNum + item.AllCount;
                            }

                            if (MainList[0].Slot >= MainList[0].SlotUnit)
                            {
                                MainList[0].SlotReset();
                                for (var i = 1; i < MainList.Count; i++)
                                {
                                    MainList[i].SlotUpdate(MainList[i - 1].Unit);
                                    if (MainList[i].Slot >= MainList[i].SlotUnit)
                                    {
                                        MainList[i].OtherUpdate(Counter);
                                        MainList[i].SlotReset();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
#if UNITY_EDITOR
            finally
            {
                Debug.Log("定时器任务全部完成 线程关闭");
            }
#endif
        }
    }
}