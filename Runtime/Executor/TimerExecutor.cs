#region

using System;
using System.Diagnostics;
using System.Text;
using Debug = UnityEngine.Debug;

#endregion

namespace AIO
{
    /// <summary>
    /// 定时任务处理器
    /// </summary>
    internal abstract class TimerExecutor<T> : ITimerExecutor<T>
    where T : Delegate
    {
        /// <summary>
        /// 定时计算器
        /// </summary>
        /// <param name="duration">定时长度 单位为毫秒</param>
        /// <param name="loop">循环次数</param>
        /// <param name="createTime">创建时间</param>
        /// <param name="tid">识别ID</param>
        protected TimerExecutor(long duration, int loop, long createTime, long tid)
        {
            Watch      = Stopwatch.StartNew();
            Loop       = loop;
            Duration   = duration;
            CreateTime = createTime;
            EndTime    = duration + createTime;
            Number     = 0;
            Interval   = 0;
            TID        = tid;
            if (TID == 0) return;
            if (TimerSystem.TimerExecutors.ContainsKey(tid))
                Debug.LogErrorFormat("TimerSystem.PushLoop: {0} already exists", tid);
            else TimerSystem.TimerExecutors.Add(tid, this);
        }

        /// <summary>
        /// 定时计算器
        /// </summary>
        /// <param name="duration">定时长度 单位为毫秒</param>
        /// <param name="loop">循环次数</param>
        /// <param name="createTime">创建时间</param>
        protected TimerExecutor(long duration, int loop, long createTime)
        {
            Watch      = Stopwatch.StartNew();
            Loop       = loop;
            Duration   = duration;
            CreateTime = createTime;
            EndTime    = duration + createTime;
            Number     = 0;
            Interval   = 0;
        }

        protected long TID { get; set; }

        internal int Loop { get; set; }

        internal byte OperatorIndex { get; set; }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        private long CurrentTime { get; set; }

        #region ITimerExecutor<T> Members

        long ITimerExecutor.TID
        {
            get => TID;
            set => TID = value;
        }

        public long CreateTime { get; private set; }

        public long Duration { get; }

        public long EndTime { get; private set; }

        public long Interval { get; private set; }

        int ITimerExecutor.Loop
        {
            get => Loop;
            set => Loop = value;
        }

        public uint Number { get; private set; }

        byte ITimerExecutor.OperatorIndex
        {
            get => OperatorIndex;
            set => OperatorIndex = value;
        }

        public Stopwatch Watch { get; private set; }

        public T Delegates { get; protected set; }

        /// <summary>
        /// 更新循环次数
        /// 返回Ture: 可以循环
        /// 返回False:循环结束
        /// </summary>
        public virtual bool UpdateLoop()
        {
            if (Loop > 1)
            {
                Number      = Number + 1; //次数增加
                CurrentTime = Watch.ElapsedMilliseconds;
                Interval    = CurrentTime - Number * Duration;
                CreateTime  = EndTime;
                EndTime     = Duration + CreateTime - Interval;
                Loop--;
                return true;
            }

            if (Loop == 1 || Loop == 0)
            {
                Number      = Number + 1; //次数增加
                CurrentTime = Watch.ElapsedMilliseconds;
                Interval    = CurrentTime - Number * Duration;
                Watch.Stop();
                Watch = null;
                if (TID != 0 && TimerSystem.TimerExecutors.ContainsKey(TID))
                    TimerSystem.TimerExecutors.Remove(TID);
                return false; //达到次数
            }

            if (Loop == -1) //无限循环
            {
                Number      = Number + 1; //次数增加
                CurrentTime = Watch.ElapsedMilliseconds;
                Interval    = CurrentTime - Number * Duration;
                CreateTime  = EndTime;
                EndTime     = Duration + CreateTime - Interval;
                return true;
            }

            if (Loop == -2) //不执行回调结束
            {
                Watch.Stop();
                Watch = null;
                return false;
            }

            return false;
        }

        public int CompareTo(ITimerExecutor other)
        {
            if (EndTime < other.EndTime) return 1;
            if (EndTime > other.EndTime) return -1;
            return 0;
        }

        public void Dispose()
        {
            Delegates = null;
        }

        public sealed override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("定时单位").Append('=').Append(Duration.ToString("00000000ms")).Append(' ');
            builder.Append("初始层级").Append('=').Append(OperatorIndex).Append(' ');
            builder.Append("创建时间").Append('=').Append(CreateTime.ToString("00000000ms")).Append(' ');
            builder.Append("结束时间").Append('=').Append(EndTime.ToString("00000000ms")).Append(' ');
            builder.Append("循环次数").Append('=').Append(Number.ToString("00000")).Append(' ');
            builder.Append("实际时间").Append('=').Append(CurrentTime.ToString("00000000ms")).Append(' ');
            builder.Append("误差时间").Append('=').Append(Interval.ToString("00000ms")).Append(' ');
            return builder.ToString();
        }

        public void Execute()
        {
            if (Loop == -2 || Delegates is null)
            {
                Dispose();
                return;
            }

            xExecute();

            if (Loop == 0) Dispose();
        }

        #endregion

        protected abstract void xExecute();
    }
}