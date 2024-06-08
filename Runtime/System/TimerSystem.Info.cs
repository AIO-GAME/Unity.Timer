#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

#endregion

namespace AIO
{
    public static class TimerSystemSettings
    {
        #region Delegates

        /// <summary>
        /// 定时器单位回调
        /// </summary>
        public delegate void TimerUnitsTask(List<(long, long, long)> units);

        #endregion

        /// <summary>
        /// 开启 循环任务线程
        /// </summary>
        public static bool EnableLoopThread { get; set; } = true;

        /// <summary>
        /// 计时器检测单位 ms
        /// </summary>
        public static long DistanceUnit { get; set; } = Unit.Time.SECOND * 2;

        /// <summary>
        /// 计时器单位回调
        /// </summary>
        public static event TimerUnitsTask TimingUnitsEvent;

        internal static void Invoke(List<(long, long, long)> units)
        {
            if (TimingUnitsEvent is null) Day(units);
            else TimingUnitsEvent.Invoke(units);
        }

        public static void Week(List<(long, long, long)> units)
        {
            units.Add((Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add((Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
            units.Add((Unit.Time.MS_HOUR, Unit.Time.MS_MIN, 60));
            units.Add((Unit.Time.MS_DAY, Unit.Time.MS_HOUR, 24));
            units.Add((Unit.Time.MS_WEEK, Unit.Time.MS_DAY, 7));
        }

        public static void Day(List<(long, long, long)> units)
        {
            units.Add((Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add((Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
            units.Add((Unit.Time.MS_HOUR, Unit.Time.MS_MIN, 60));
            units.Add((Unit.Time.MS_DAY, Unit.Time.MS_HOUR, 24));
        }

        public static void Hour(List<(long, long, long)> units)
        {
            units.Add((Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add((Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
            units.Add((Unit.Time.MS_HOUR, Unit.Time.MS_MIN, 60));
        }

        public static void Min(List<(long, long, long)> units)
        {
            units.Add((Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add((Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
        }

        public static void Second(List<(long, long, long)> units)
        {
            units.Add((Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
        }
    }

    /// <summary>
    /// 定时器 时间调度器
    /// </summary>
    partial class TimerSystem
    {
        /// <summary>
        /// 计时器 精确时间刻度器
        /// </summary>
        [ContextStatic]
        private static Stopwatch Watch;

        /// <summary>
        /// 多层级定时器 主 有添加接口 有消耗 只有一个
        /// </summary>
        [ContextStatic]
        private static List<ITimerOperator> MainList;

        /// <summary>
        /// 多层级定时器 Task 副 没有添加接口 只有消耗 有多个
        /// </summary>
        [ContextStatic]
        private static List<ITimerContainer> TaskList;

        /// <summary>
        /// 无限循环定时器容器
        /// </summary>
        [ContextStatic]
        private static ITimerContainer LoopContainer;

        /// <summary>
        /// 当前容器列表剩余数量
        /// </summary>
        [ContextStatic]
        private static volatile int RemainNum;

        /// <summary>
        /// 更新刷新List时间
        /// </summary>
        [ContextStatic]
        internal static long UPDATELISTTIME;

        /// <summary>
        /// 容量
        /// </summary>
        [ContextStatic]
        private static volatile int Capacity;

        /// <summary>
        /// 更新刷新List时间
        /// </summary>
        [ContextStatic]
        private static long UpdateCacheTime;

        /// <summary>
        /// 线程句柄
        /// </summary>
        [ContextStatic]
        private static Task ThreadHandle;

        [ContextStatic]
        private static CancellationToken TaskHandleToken;

        [ContextStatic]
        private static CancellationTokenSource TaskHandleTokenSource;

        internal static List<(long, long, long)> TimingUnits { get; private set; }

        internal static Dictionary<long, ITimerExecutor> TimerExecutors { get; private set; }

        /// <summary>
        /// 当前计时器计算单位 ms
        /// </summary>
        public static long Unit { get; private set; }

        /// <summary>
        /// 计数器 代表当前程序运行时间 单位ms
        /// </summary>
        public static long Counter { get; private set; }

        /// <summary>
        /// 开关
        /// </summary>
        internal static bool SWITCH { get; private set; }

        /// <summary>
        /// 获取定时器信息
        /// </summary>
        public new static void ToString()
        {
            var builder = new StringBuilder();
            builder.Append("-----------<主定时器 层器>-----------").AppendLine();
            lock (MainList)
            {
                foreach (var item in MainList) builder.Append(item).AppendLine().AppendLine();
            }

            builder.Append("-----------<无限循环 层级>-----------").AppendLine();
            lock (LoopContainer)
            {
                builder.Append(LoopContainer).AppendLine();
            }

            builder.Append("-----------<辅助执行 层级>-----------").AppendLine();
            lock (TaskList)
            {
                foreach (var item in TaskList) builder.Append(item).AppendLine();
            }

            Debug.Log(builder.ToString());
        }
    }
}