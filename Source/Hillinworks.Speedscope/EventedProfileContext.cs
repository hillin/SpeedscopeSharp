using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Hillinworks.Speedscope.FileFormat;

namespace Hillinworks.Speedscope
{
    public class EventedProfileContext : SubProfileContextBase
    {
        internal ConcurrentDictionary<EventHandle, bool> EventHandles { get; }
            = new ConcurrentDictionary<EventHandle, bool>();
        internal ConcurrentBag<IEvent> Events { get; }
            = new ConcurrentBag<IEvent>();

        private EventHandle OpenEvent(Frame frame)
        {
            var now = DateTime.UtcNow;
            return this.EnsureNotFrozen(
                () =>
                {
                    var handle = new EventHandle(this, frame, now);
                    var addResult = this.EventHandles.TryAdd(handle, true);
                    Debug.Assert(addResult);
                    return handle;
                });
        }

        public EventHandle OpenSimpleEvent(string frameName)
        {
            return this.OpenEvent(Frame.CreateSimple(frameName));
        }

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public EventHandle OpenEvent(
            [CallerMemberName] string? name = null,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int line = -1)
        {
            return this.OpenEvent(Frame.FromSource(name, file, line));
        }

        public void CloseEvent(EventHandle eventHandle)
        {
            var now = DateTime.UtcNow;

            this.EnsureNotFrozen(
                () =>
                {
                    var frameIndex = this.ProfileContext.TryAddFrame(eventHandle.Frame);
                    var openEvent = new OpenFrameEvent(
                        this.GetTimeValue(eventHandle.StartTime),
                        frameIndex
                    );

                    this.Events.Add(openEvent);
                    var removeResult = this.EventHandles.TryRemove(eventHandle, out var value);
                    Debug.Assert(removeResult && value);

                    var epsilon = (long)(1 / this.TickFactor * 0.001);   // 0.001 units

                    if (now.Ticks - eventHandle.StartTime.Ticks <= epsilon)
                    {
                        // too close, pretend as if it was never happened
                        return;
                    }

                    // make some gap between this event and whatever event which may be open in the 
                    // same time, otherwise speedscope may (mistakenly) treat this event as the parent
                    // of the upcoming event
                    now = now.AddTicks(-epsilon);

                    var closeEvent = new CloseFrameEvent(
                        this.GetTimeValue(now),
                        frameIndex);

                    this.Events.Add(closeEvent);
                });
        }

        internal override void Freeze()
        {
            base.Freeze();

            if (this.EventHandles.Any())
            {
                throw new InvalidOperationException("some events are still not closed");
            }

            this.EventHandles.Clear();
        }

        internal override bool IsEmpty => this.Events.Count == 0;

        internal override ProfileBase CreateProfile()
        {
            this.CheckFrozen();

            Debug.Assert(!this.IsEmpty);

            var events = this.Events.OrderBy(e => e.At).ToList();

            var startValue = events[0].At;
            var endValue = events[events.Count - 1].At;

            Debug.Assert(endValue > startValue);

            // leave some blank space after ended
            endValue += (endValue - startValue) * 0.2;

            return new EventedProfile(
                this.Name,
                startValue,
                endValue,
                (ValueUnit)this.TimeUnit,
                events);
        }

        public EventedProfileContext(
            ProfileContext profileContext,
            string name,
            TimeUnit timeUnit)
            : base(profileContext, name, timeUnit)
        {
        }
    }
}