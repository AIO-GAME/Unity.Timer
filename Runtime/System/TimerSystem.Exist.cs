#region

using System;
using System.Collections.Generic;

#endregion

namespace AIO
{
    partial class TimerSystem
    {
        /// <summary>
        /// 判断任务是否存在
        /// </summary>
        public static bool Exist(long key) { return TimerExecutors.ContainsKey(key); }

        /// <summary>
        /// 判断任务是否存在
        /// </summary>
        public static bool Exist(string key) { return TimerExecutors.ContainsKey(key.GetHashCode()); }

        /// <summary>
        /// 判断任务是否存在
        /// </summary>
        public static bool Exist<E>(E key)
        where E : Enum => TimerExecutors.ContainsKey(key.GetHashCode());

        public static ITimerExecutor Get(long key) { return TimerExecutors.GetValueOrDefault(key, null); }

        public static ITimerExecutor Get(string key) { return TimerExecutors.GetValueOrDefault(key.GetHashCode(), null); }

        public static ITimerExecutor Get<E>(E key)
        where E : Enum => TimerExecutors.GetValueOrDefault(key.GetHashCode(), null);
    }
}