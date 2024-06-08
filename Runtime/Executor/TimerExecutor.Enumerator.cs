#region

using System;
using System.Collections;
using UnityEngine;

#endregion

namespace AIO
{
    /// <summary>
    /// 定时任务处理器
    /// </summary>
    internal class TimerExecutorEnumerator : TimerExecutor<Func<IEnumerator>>
    {
        /// <summary>
        /// 定时计算器
        /// </summary>
        /// <param name="duration">定时长度 单位为毫秒</param>
        /// <param name="loop">循环次数</param>
        /// <param name="createTime">创建时间</param>
        /// <param name="delegateValue">委托函数</param>
        internal TimerExecutorEnumerator(long duration, int loop, long createTime, Func<IEnumerator> delegateValue) :
            base(duration, loop, createTime)
        {
            Delegates = delegateValue;
        }

        /// <summary>
        /// 定时计算器
        /// </summary>
        /// <param name="tid">识别ID</param>
        /// <param name="duration">定时长度 单位为毫秒</param>
        /// <param name="loop">循环次数</param>
        /// <param name="createTime">创建时间</param>
        /// <param name="delegateValue">委托函数</param>
        internal TimerExecutorEnumerator(long              tid,
                                         long              duration,
                                         int               loop,
                                         long              createTime,
                                         Func<IEnumerator> delegateValue) : base(duration, loop, createTime, tid)
        {
            Delegates = delegateValue;
        }

        protected override void xExecute()
        {
            try
            {
                Runner.StartCoroutine(Delegates);
            }
            catch (Exception e)
            {
                Debug.LogError("TimerExecutorEnumerator Execute Error: " + e);
            }
        }
    }
}