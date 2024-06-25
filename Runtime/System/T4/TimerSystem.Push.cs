using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIO
{
    partial class TimerSystem
    {
        #region Push

        #region Push Func<IEnumerator>

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void Push(int key, long duration, Func<IEnumerator> delegateValue, ushort loop = 1) =>
            AddUpdate(new TimerExecutorEnumerator(key, duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void Push(long key, long duration, Func<IEnumerator> delegateValue, ushort loop = 1) =>
            AddUpdate(new TimerExecutorEnumerator(key, duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void Push<E>(E key, long duration, Func<IEnumerator> delegateValue, ushort loop = 1)
        where E : Enum => AddUpdate(new TimerExecutorEnumerator(key.GetHashCode(), duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void Push(Guid key, long duration, Func<IEnumerator> delegateValue, ushort loop = 1) =>
            AddUpdate(new TimerExecutorEnumerator(key.GetHashCode(), duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void Push(string key, long duration, Func<IEnumerator> delegateValue, ushort loop = 1)
        {
            if (string.IsNullOrEmpty(key)) return;
            AddUpdate(new TimerExecutorEnumerator(key.GetHashCode(), duration, loop == 0 ? -1 : loop, Counter, delegateValue));
        }

        #endregion

        #region Push Action

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void Push(int key, long duration, Action delegateValue, ushort loop = 1) =>
            AddUpdate(new TimerExecutorAction(key, duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void Push(long key, long duration, Action delegateValue, ushort loop = 1) =>
            AddUpdate(new TimerExecutorAction(key, duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void Push<E>(E key, long duration, Action delegateValue, ushort loop = 1)
        where E : Enum => AddUpdate(new TimerExecutorAction(key.GetHashCode(), duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void Push(Guid key, long duration, Action delegateValue, ushort loop = 1) =>
            AddUpdate(new TimerExecutorAction(key.GetHashCode(), duration, loop == 0 ? -1 : loop, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        /// <param name="loop"> <c>0:循环</c> <c>1:循环次数</c> </param>
        public static void Push(string key, long duration, Action delegateValue, ushort loop = 1)
        {
            if (string.IsNullOrEmpty(key)) return;
            AddUpdate(new TimerExecutorAction(key.GetHashCode(), duration, loop == 0 ? -1 : loop, Counter, delegateValue));
        }

        #endregion

        #endregion

        #region PushOnce

        #region PushOnce Func<IEnumerator>

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce(int key, long duration, Func<IEnumerator> delegateValue) =>
            AddUpdate(new TimerExecutorEnumerator(key, duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce(long key, long duration, Func<IEnumerator> delegateValue) =>
            AddUpdate(new TimerExecutorEnumerator(key, duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce<E>(E key, long duration, Func<IEnumerator> delegateValue)
        where E : Enum => AddUpdate(new TimerExecutorEnumerator(key.GetHashCode(), duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce(Guid key, long duration, Func<IEnumerator> delegateValue) =>
            AddUpdate(new TimerExecutorEnumerator(key.GetHashCode(), duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce(string key, long duration, Func<IEnumerator> delegateValue)
        {
            if (string.IsNullOrEmpty(key)) return;
            AddUpdate(new TimerExecutorEnumerator(key.GetHashCode(), duration, 1, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (一次性)(无键值) </remarks>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce(long duration, Func<IEnumerator> delegateValue) =>
            AddUpdate(new TimerExecutorEnumerator(duration, 1, Counter, delegateValue));

        #endregion

        #region PushOnce Action

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce(int key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorAction(key, duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce(long key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorAction(key, duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce<E>(E key, long duration, Action delegateValue)
        where E : Enum => AddUpdate(new TimerExecutorAction(key.GetHashCode(), duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce(Guid key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorAction(key.GetHashCode(), duration, 1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器
        /// </summary>
        /// <remarks> (一次性) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce(string key, long duration, Action delegateValue)
        {
            if (string.IsNullOrEmpty(key)) return;
            AddUpdate(new TimerExecutorAction(key.GetHashCode(), duration, 1, Counter, delegateValue));
        }

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (一次性)(无键值) </remarks>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushOnce(long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorAction(duration, 1, Counter, delegateValue));

        #endregion

        #endregion

        #region PushLoop

        #region PushLoop Func<IEnumerator>

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoop(int key, long duration, Func<IEnumerator> delegateValue) =>
            AddUpdate(new TimerExecutorEnumerator(key, duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoop(long key, long duration, Func<IEnumerator> delegateValue) =>
            AddUpdate(new TimerExecutorEnumerator(key, duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoop<E>(E key, long duration, Func<IEnumerator> delegateValue)
        where E : Enum => AddUpdate(new TimerExecutorEnumerator(key.GetHashCode(), duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoop(Guid key, long duration, Func<IEnumerator> delegateValue) =>
            AddUpdate(new TimerExecutorEnumerator(key.GetHashCode(), duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoop(string key, long duration, Func<IEnumerator> delegateValue)
        {
            if (string.IsNullOrEmpty(key)) return;
            AddUpdate(new TimerExecutorEnumerator(key.GetHashCode(), duration, -1, Counter, delegateValue));
        }

        #endregion

        #region PushLoop Action

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoop(int key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorAction(key, duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoop(long key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorAction(key, duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoop<E>(E key, long duration, Action delegateValue)
        where E : Enum => AddUpdate(new TimerExecutorAction(key.GetHashCode(), duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoop(Guid key, long duration, Action delegateValue) =>
            AddUpdate(new TimerExecutorAction(key.GetHashCode(), duration, -1, Counter, delegateValue));

        /// <summary>
        /// 添加定时任务处理器 
        /// </summary>
        /// <remarks> (循环) </remarks>
        /// <param name="key"> 键值 </param>
        /// <param name="duration"> 间隔时间 </param>
        /// <param name="delegateValue"> 委托 </param>
        public static void PushLoop(string key, long duration, Action delegateValue)
        {
            if (string.IsNullOrEmpty(key)) return;
            AddUpdate(new TimerExecutorAction(key.GetHashCode(), duration, -1, Counter, delegateValue));
        }

        #endregion

        #endregion

    }
}
