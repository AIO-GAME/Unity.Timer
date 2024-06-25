#region

using System;
using System.Collections;

#endregion

namespace AIO
{
    partial class TimerSystem
    {
        /// <summary>
        /// 取出循环任务
        /// </summary>
        /// <param name="key"> 任务ID </param>
        public static void Pop(long key)
        {
            if (!TimerExecutors.TryGetValue(key, out var executor)) return;
            executor.Loop = -2;
            TimerExecutors.Remove(key);
        }

        /// <summary>
        /// 取出循环任务
        /// </summary>
        /// <param name="key"> 任务ID </param>
        public static void Pop(string key)
        {
            if (string.IsNullOrEmpty(key)) return;
            Pop(key.GetHashCode());
        }

        /// <summary>
        /// 取出循环任务
        /// </summary>
        /// <param name="key"> 任务ID </param>
        public static void Pop<E>(E key) where E : Enum { Pop(key.GetHashCode()); }
    }
}
