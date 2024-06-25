#region

using System;

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
        public static bool Exist<E>(E key) where E : Enum { return TimerExecutors.ContainsKey(key.GetHashCode()); }
    }
}
