#region

using System;
using System.Diagnostics;

#endregion

namespace AIO
{
    /// <summary>
    /// 定时任务处理器
    /// </summary>
    public interface ITimerExecutor : IComparable<ITimerExecutor>, IDisposable
    {
        /// <summary>
        /// 定时器索引
        /// </summary>
#if UNITY_2021_1_OR_NEWER
        long TID { get; protected set; }
#else
        long TID { get; set; }
#endif
        /// <summary>
        /// 创建时间 单位毫秒
        /// </summary>
        long CreateTime { get; }

        /// <summary>
        /// 当前任务总计数 单位1ms
        /// </summary>
        long Duration { get; }

        /// <summary>
        /// 终止时间 单位毫秒
        /// </summary>
        long EndTime { get; }

        /// <summary>
        /// 误差时间 单位毫秒
        /// </summary>
        long Interval { get; }

        /// <summary>
        /// 循环次数
        /// -2:回调已经被删除 无法再次执行
        /// -1:代表无限
        /// =0:代表执行一次
        /// >0:代表循环次数
        /// </summary>
        int Loop { get; set; }

        /// <summary>
        /// 当前任务实际循环次数
        /// </summary>
        uint Number { get; }

        /// <summary>
        /// 操作索引
        /// </summary>
        byte OperatorIndex { get; set; }

        /// <summary>
        /// 精度器 记录当前任务实际持续时间
        /// </summary>
        Stopwatch Watch { get; }

        /// <summary>
        /// 更新循环次数
        /// 返回Ture: 可以循环
        /// 返回False:循环结束
        /// </summary>
        bool UpdateLoop();

        /// <summary>
        /// 输出信息
        /// </summary>
        string ToString();

        /// <summary>
        /// 执行回调
        /// </summary>
        [DebuggerHidden, DebuggerStepThrough]
        void Execute();
    }

    /// <summary>
    /// 定时任务处理器
    /// </summary>
    internal interface ITimerExecutor<out T> : ITimerExecutor
    where T : Delegate
    {
        /// <summary>
        /// 当前携带任务
        /// </summary>
        T Delegates { get; }
    }
}