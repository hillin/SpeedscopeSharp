using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hillinworks.Speedcore.FileFormat
{
    internal class EventedProfile : ProfileBase
    {
        public override ProfileType Type => ProfileType.Evented;

        [JsonProperty("events")]
        public List<IEvent> Events { get; }

        [JsonConstructor]
        public EventedProfile(
            string name,
            double startValue,
            double endValue,
            ValueUnit unit,
            List<IEvent> events)
            : base(name, startValue, endValue, unit)
        {
            this.Events = events;
        }

        public EventedProfile(
            string name,
            double startValue,
            double endValue,
            ValueUnit unit)
            : this(name, startValue, endValue, unit, new List<IEvent>())
        {
        }
    }
}