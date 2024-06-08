using System.Collections.Generic;

namespace AIO
{
    public static class TimerSystemSettings
    {
        #region Delegates

        /// <summary>
        /// 定时器单位回调
        /// </summary>
        public delegate void TimerUnitsTask(ICollection<(long, long, long)> units);

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

        internal static void Invoke(ICollection<(long, long, long)> units)
        {
            if (TimingUnitsEvent is null) Day(units);
            else TimingUnitsEvent.Invoke(units);
        }

        public static void Week(ICollection<(long, long, long)> units)
        {
            units.Add((Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add((Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
            units.Add((Unit.Time.MS_HOUR, Unit.Time.MS_MIN, 60));
            units.Add((Unit.Time.MS_DAY, Unit.Time.MS_HOUR, 24));
            units.Add((Unit.Time.MS_WEEK, Unit.Time.MS_DAY, 7));
        }

        public static void Day(ICollection<(long, long, long)> units)
        {
            units.Add((Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add((Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
            units.Add((Unit.Time.MS_HOUR, Unit.Time.MS_MIN, 60));
            units.Add((Unit.Time.MS_DAY, Unit.Time.MS_HOUR, 24));
        }

        public static void Hour(ICollection<(long, long, long)> units)
        {
            units.Add((Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add((Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
            units.Add((Unit.Time.MS_HOUR, Unit.Time.MS_MIN, 60));
        }

        public static void Min(ICollection<(long, long, long)> units)
        {
            units.Add((Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add((Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
        }

        public static void Second(ICollection<(long, long, long)> units) { units.Add((Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit)); }
    }
}