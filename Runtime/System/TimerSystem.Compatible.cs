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
        public static void Pop(long tid)
        {
            if (!TimerExecutors.ContainsKey(tid)) return;
            TimerExecutors[tid].Loop = -2;
            TimerExecutors.Remove(tid);
        }

        /// <summary>
        /// 取出循环任务
        /// </summary>
        public static void Pop(string tidstrtiding)
        {
            var tid = tidstrtiding.GetHashCode();
            if (TimerExecutors.ContainsKey(tid))
            {
                TimerExecutors[tid].Loop = -2;
                TimerExecutors.Remove(tid);
            }
        }

        /// <summary>
        /// 判断任务是否存在
        /// </summary>
        public static bool Exist(long tid)
        {
            return TimerExecutors.ContainsKey(tid);
        }

        /// <summary>
        /// 判断任务是否存在
        /// </summary>
        public static bool Exist(string tid)
        {
            return TimerExecutors.ContainsKey(tid.GetHashCode());
        }

        #region Timer Executor Enumerator

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        public static void Push(long duration, Func<IEnumerator> delegateValue)
        {
            AddUpdate(new TimerExecutorEnumerator(duration, 1, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        public static void PushLoop(long duration, Func<IEnumerator> delegateValue)
        {
            AddUpdate(new TimerExecutorEnumerator(duration, -1, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器 是否循环 0循环 1循环次数
        /// </summary>
        public static void Push(long duration, ushort loop, Func<IEnumerator> delegateValue)
        {
            AddUpdate(new TimerExecutorEnumerator(duration, loop == 0 ? -1 : loop, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器 是否循环 0循环 1循环次数
        /// </summary>
        public static void Push(long duration, byte loop, Func<IEnumerator> delegateValue)
        {
            AddUpdate(new TimerExecutorEnumerator(duration, loop == 0 ? -1 : loop, Counter, delegateValue));
        }

        #endregion

        #region Timer Executor Action

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        public static void Push(long duration, Action delegateValue)
        {
            AddUpdate(new TimerExecutorAction(duration, 1, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        public static void Push(long tid, long duration, Action delegateValue)
        {
            AddUpdate(new TimerExecutorAction(tid, duration, 1, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        public static void Push(string tid, long duration, Action delegateValue)
        {
            AddUpdate(new TimerExecutorAction(tid.GetHashCode(), duration, 1, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        public static void PushLoop(long duration, Action delegateValue)
        {
            AddUpdate(new TimerExecutorAction(duration, -1, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        public static void PushLoop(long tid, long duration, Action delegateValue)
        {
            AddUpdate(new TimerExecutorAction(tid, duration, -1, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        public static void PushLoop(string tid, long duration, Action delegateValue)
        {
            AddUpdate(new TimerExecutorAction(tid.GetHashCode(), duration, -1, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器 是否循环 0循环 1循环次数
        /// </summary>
        public static void Push(long duration, ushort loop, Action delegateValue)
        {
            AddUpdate(new TimerExecutorAction(duration, loop == 0 ? -1 : loop, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器 是否循环 0循环 1循环次数
        /// </summary>
        public static void Push(long duration, byte loop, Action delegateValue)
        {
            AddUpdate(new TimerExecutorAction(duration, loop == 0 ? -1 : loop, Counter, delegateValue));
        }

        #endregion
    }
}