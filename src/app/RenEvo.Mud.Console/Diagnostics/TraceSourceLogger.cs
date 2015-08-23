using System;
using System.Diagnostics;
using Common.Logging;
using Common.Logging.Factory;

namespace RenEvo.Mud.Diagnostics
{
    public class TraceSourceLoggerFactory : AbstractCachingLoggerFactoryAdapter
    {
        protected override ILog CreateLogger(string name)
        {
            return new TraceSourceLogger(name);
        }
    }

    public class TraceSourceLogger : AbstractLogger
    {
        private TraceSource _innerSource;

        public TraceSourceLogger(string name)
        {
            _innerSource = new TraceSource(name, SourceLevels.All);
        }

        public override bool IsDebugEnabled
        {
            get
            {
                return _innerSource.Switch.Level.HasFlag(SourceLevels.Verbose);
            }
        }

        public override bool IsErrorEnabled
        {
            get
            {
                return _innerSource.Switch.Level.HasFlag(SourceLevels.Error);
            }
        }

        public override bool IsFatalEnabled
        {
            get
            {
                return _innerSource.Switch.Level.HasFlag(SourceLevels.Critical);
            }
        }

        public override bool IsInfoEnabled
        {
            get
            {
                return _innerSource.Switch.Level.HasFlag(SourceLevels.Information);
            }
        }

        public override bool IsTraceEnabled
        {
            get
            {
                return _innerSource.Switch.Level.HasFlag(SourceLevels.Verbose);
            }
        }

        public override bool IsWarnEnabled
        {
            get
            {
                return _innerSource.Switch.Level.HasFlag(SourceLevels.Warning);
            }
        }

        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            _innerSource.TraceData(GetEventType(level), (int)level, message);

        }

        private TraceEventType GetEventType(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug: return TraceEventType.Verbose;
                case LogLevel.Trace: return TraceEventType.Verbose;
                case LogLevel.Info: return TraceEventType.Information;
                case LogLevel.Warn: return TraceEventType.Warning;
                case LogLevel.Error: return TraceEventType.Error;
                case LogLevel.Fatal: return TraceEventType.Critical;
                case LogLevel.All: return TraceEventType.Verbose;
                case LogLevel.Off: return TraceEventType.Verbose;
            }

            return TraceEventType.Verbose;
        }
    }
}
