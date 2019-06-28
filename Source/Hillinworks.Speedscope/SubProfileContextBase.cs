using System;
using Hillinworks.Speedscope.FileFormat;

namespace Hillinworks.Speedscope
{
    public abstract class SubProfileContextBase : FreezableObject
    {
        protected ProfileContext ProfileContext { get; }
        public string Name { get; }
        public TimeUnit TimeUnit { get; }
        private double TickFactor { get; }
        internal abstract bool IsEmpty { get; }

        protected double GetTimeValue(DateTime time)
        {
            return (time.Ticks - this.ProfileContext.BaseTime.Ticks) * this.TickFactor;
        }

        protected SubProfileContextBase(ProfileContext profileContext, string name, TimeUnit timeUnit)
        {
            this.TickFactor = timeUnit.GetTickFactor();

            this.ProfileContext = profileContext;
            this.Name = name;
            this.TimeUnit = timeUnit;
        }

        internal abstract ProfileBase CreateProfile();
    }
}