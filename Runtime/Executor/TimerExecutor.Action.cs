#region

using System;

#endregion

namespace AIO
{
    /// <summary>
    /// 定时任务处理器
    /// </summary>
    internal sealed class TimerExecutorAction : TimerExecutor<Action>
    {
        /// <summary>
        /// 定时计算器
        /// </summary>
        /// <param name="duration">定时长度 单位为毫秒</param>
        /// <param name="loop">循环次数</param>
        /// <param name="createTime">创建时间</param>
        /// <param name="delegateValue">委托函数</param>
        internal TimerExecutorAction(long duration, int loop, long createTime, Action delegateValue) : base(duration, loop, createTime)
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
        internal TimerExecutorAction(long tid, long duration, int loop, long createTime, Action delegateValue) : base(duration, loop, createTime, tid)
        {
            Delegates = delegateValue;
        }

        protected override void xExecute() { Delegates.Invoke(); }
    }
}