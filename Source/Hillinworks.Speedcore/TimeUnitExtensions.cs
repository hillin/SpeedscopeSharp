using System;

namespace Hillinworks.Speedcore
{
    internal static class TimeUnitExtensions
    {
        public static double GetTickFactor(this TimeUnit timeUnit)
        {
            switch (timeUnit)
            {
                case TimeUnit.Nanoseconds:
                    return 1e2;

                case TimeUnit.Microseconds:
                    return 1e-1;

                case TimeUnit.Milliseconds:
                    return 1e-4;

                case TimeUnit.Seconds:
                    return 1e-7;

                default:
                    throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null);
            }
        }
    }
}