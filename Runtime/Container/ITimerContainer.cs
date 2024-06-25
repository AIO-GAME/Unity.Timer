#region

using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace AIO
{
    /// <summary>
    /// 辅助定时器容器
    /// </summary>
    internal interface ITimerContainer : IDisposable
    {
        /// <summary>
        /// 定时器ID
        /// </summary>
        int ID { get; }

        /// <summary>
        /// 定时器索引
        /// </summary>
        ITimerOperator this[int index] { get; }

        /// <summary>
        /// 精度表
        /// </summary>
        Stopwatch Watch { get; }

        /// <summary>
        /// 容器列表
        /// </summary>
        List<ITimerOperator> List { get; }

        /// <summary>
        /// 精度单位
        /// </summary>
        long Unit { get; }

        /// <summary>
        /// 当前时间
        /// </summary>
        long Counter { get; }

        /// <summary>
        /// 剩余定时器任务数量
        /// </summary>
        int RemainNum { get; }

        /// <summary>
        /// 更新刷新List时间
        /// </summary>
        long UpdateCacheTime { get; }

        /// <summary>
        /// 取消定时器
        /// </summary>
        void Cancel();

        /// <summary>
        /// 开始定时器
        /// </summary>
        void Start();

        /// <summary>
        /// 重置定时器
        /// </summary>
        string ToString();

        /// <summary>
        /// 推送更新
        /// </summary>
        /// <param name="timer">执行器</param>
        void PushUpdate(ITimerExecutor timer);

        /// <summary>
        /// 推送更新
        /// </summary>
        void PushUpdate(List<ITimerExecutor> timer);
    }
}
