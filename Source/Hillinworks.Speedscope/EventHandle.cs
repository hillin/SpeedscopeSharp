using System;

namespace Hillinworks.Speedscope
{
    public class EventHandle : IDisposable
    {
        internal Frame Frame { get; }
        public DateTime StartTime { get; }
        public EventedProfileContext ProfileContext { get; }

        internal EventHandle(EventedProfileContext profileContext, Frame frame, DateTime startTime)
        {
            this.ProfileContext = profileContext;
            this.Frame = frame;
            this.StartTime = startTime;
        }

        void IDisposable.Dispose()
        {
            this.ProfileContext.CloseEvent(this);
        }
    }
}