#region

using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

#if !UNITY_WEBGL
using System.Threading;
using System.Threading.Tasks;
#endif

#endregion

namespace AIO
{
    /// <summary>
    /// 定时器 时间调度器
    /// </summary>
    public static partial class TimerSystem
    {
        private static bool IsInitialize;

        /// <summary>
        /// 定时器单位回调
        /// </summary>
        /// <param name="updateLimit">更新限制</param>
        /// <param name="capacity">容量</param>
        public static void Initialize(long updateLimit = 10, int capacity = 1024 * 8)
        {
            if (IsInitialize) return;
            Watch = Stopwatch.StartNew();

            //保持当前计算单位是毫秒 因为当前时间单位计算底层是纳秒

            TaskList       = Pool.List<ITimerContainer>();
            MainList       = Pool.List<ITimerOperator>();
            TimingUnits    = Pool.List<(long, long, long)>();
            TimerExecutors = Pool.Dictionary<long, ITimerExecutor>();

            Unit = TimerSystemSettings.DistanceUnit;
            TimerSystemSettings.Invoke(TimingUnits);

            RemainNum       = 0;
            UpdateCacheTime = 0;
            Capacity        = capacity;
            UPDATELISTTIME  = updateLimit;

            RegisterList();

            SWITCH = true;

            TaskHandleTokenSource = new CancellationTokenSource();
            TaskHandleToken       = TaskHandleTokenSource.Token;
            ThreadHandle          = Task.Factory.StartNew(Update, TaskHandleToken);

            if (TimerSystemSettings.EnableLoopThread)
            {
                LoopContainer = new TimerContainerLoop(Unit);
                LoopContainer.Start();
            }

            IsInitialize         =  true;
            Application.quitting += Dispose;
        }

        static partial void Update();

        /// <summary>
        /// 注册主队列
        /// </summary>
        private static void RegisterList()
        {
            MainList.Clear();
            for (byte i = 0; i < TimingUnits.Count; i++) MainList.Add(new TimerOperatorAuto(i, TimingUnits[i].Item2, TimingUnits[i].Item3));
        }

        /// <summary>
        /// 释放辅助定时器
        /// </summary>
        internal static void DisposeContainer(ITimerContainer container)
        {
            if (SWITCH) TaskList.Remove(container);
        }

        private static void Dispose()
        {
            ToString();
            SWITCH = false;
            TaskList.Free();
            MainList.Free();
            TimingUnits.Free();
            TimerExecutors.Free();

            try
            {
                lock (LoopContainer)
                {
                    LoopContainer.Cancel();
                    LoopContainer = null;
                }

                lock (TaskList)
                {
                    foreach (var item in TaskList) item.Cancel();
                    TaskList.Clear();
                    TaskList = null;
                }

                lock (MainList)
                {
                    foreach (var item in MainList) item.Dispose();

                    MainList.Clear();
                    MainList = null;
                }

                if (!ThreadHandle.IsCompleted)
                    TaskHandleTokenSource.Cancel(true);
                else ThreadHandle.Dispose();

                Watch = null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                Debug.Log("定时器系统已销毁");
            }

            Application.quitting -= Dispose;
        }
    }
}