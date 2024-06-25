using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIO
{
    partial class TimerSystem
    {
        #region Push

        #region PushMain Action

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void PushMain(int key, long duration, Action delegateValue, ushort loop = 1) =>
            AddUpdate(new TimerExecutorMainThread(key, duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void PushMain(long key, long duration, Action delegateValue, ushort loop = 1) =>
            AddUpdate(new TimerExecutorMainThread(key, duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void PushMain<E>(E key, long duration, Action delegateValue, ushort loop = 1)
        where E : Enum => AddUpdate(new TimerExecutorMainThread(key.GetHashCode(), duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void PushMain(Guid key, long duration, Action delegateValue, ushort loop = 1) =>
            AddUpdate(new TimerExecutorMainThread(key.GetHashCode(), duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void PushMain(string key, long duration, Action delegateValue, ushort loop = 1)
        {
            if (string.IsNullOrEmpty(key)) return;
            AddUpdate(new TimerExecutorMainThread(key.GetHashCode(), duration, loop == 0 ? -1 : loop, Counter, delegateValue));
        }

        #endregion

        #endregion

        #region PushOnce

        #region PushOnce Action

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnceMain(int key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorMainThread(key, duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnceMain(long key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorMainThread(key, duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnceMain<E>(E key, long duration, Action delegateValue)
        where E : Enum => AddUpdate(new TimerExecutorMainThread(key.GetHashCode(), duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnceMain(Guid key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorMainThread(key.GetHashCode(), duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnceMain(string key, long duration, Action delegateValue)
        {
            if (string.IsNullOrEmpty(key)) return;
            AddUpdate(new TimerExecutorMainThread(key.GetHashCode(), duration, 1, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (一次性)(无键值) </remarks>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnceMain(long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorMainThread(duration, 1, Counter, delegateValue));

        #endregion

        #endregion

        #region PushLoop

        #region PushLoop Action

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoopMain(int key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorMainThread(key, duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoopMain(long key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorMainThread(key, duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoopMain<E>(E key, long duration, Action delegateValue)
        where E : Enum => AddUpdate(new TimerExecutorMainThread(key.GetHashCode(), duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoopMain(Guid key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorMainThread(key.GetHashCode(), duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoopMain(string key, long duration, Action delegateValue)
        {
            if (string.IsNullOrEmpty(key)) return;
            AddUpdate(new TimerExecutorMainThread(key.GetHashCode(), duration, -1, Counter, delegateValue));
        }

        #endregion

        #endregion
    }
}
