using System.Collections.Generic;
using UnityEngine.Scripting;

namespace AIO
{
    [Preserve]
    public class UnitItem
    {
        /// <summary>
        /// 总时间
        /// </summary>
        public long Total;

        /// <summary>
        /// 单位时间
        /// </summary>
        public long Unit;

        /// <summary>
        /// 槽位数量
        /// </summary>
        public long Slot;

        public UnitItem(long b, long d, long c)
        {
            Total = b;
            Unit  = d;
            Slot  = c;
        }
    }

    [Preserve]
    public static class TimerSystemSettings
    {
        #region Delegates

        /// <summary>
        /// 定时器单位回调
        /// </summary>
        [Preserve]
        public delegate void TimerUnitsTask(ICollection<UnitItem> units);

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
        [Preserve]
        public static event TimerUnitsTask TimingUnitsEvent;

        [Preserve]
        internal static void Invoke(ICollection<UnitItem> units)
        {
            if (TimingUnitsEvent is null) Day(units);
            else TimingUnitsEvent.Invoke(units);
        }

        [Preserve]
        public static void Week(ICollection<UnitItem> units)
        {
            units.Add(new UnitItem(Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add(new UnitItem(Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
            units.Add(new UnitItem(Unit.Time.MS_HOUR, Unit.Time.MS_MIN, 60));
            units.Add(new UnitItem(Unit.Time.MS_DAY, Unit.Time.MS_HOUR, 24));
            units.Add(new UnitItem(Unit.Time.MS_WEEK, Unit.Time.MS_DAY, 7));
        }

        [Preserve]
        public static void Day(ICollection<UnitItem> units)
        {
            units.Add(new UnitItem(Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add(new UnitItem(Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
            units.Add(new UnitItem(Unit.Time.MS_HOUR, Unit.Time.MS_MIN, 60));
            units.Add(new UnitItem(Unit.Time.MS_DAY, Unit.Time.MS_HOUR, 24));
        }

        [Preserve]
        public static void Hour(ICollection<UnitItem> units)
        {
            units.Add(new UnitItem(Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add(new UnitItem(Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
            units.Add(new UnitItem(Unit.Time.MS_HOUR, Unit.Time.MS_MIN, 60));
        }

        [Preserve]
        public static void Min(ICollection<UnitItem> units)
        {
            units.Add(new UnitItem(Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit));
            units.Add(new UnitItem(Unit.Time.MS_MIN, Unit.Time.MS_SECOND, 60));
        }

        [Preserve]
        public static void Second(ICollection<UnitItem> units) { units.Add(new UnitItem(Unit.Time.MS_SECOND, DistanceUnit, Unit.Time.MS_SECOND / DistanceUnit)); }
    }
}