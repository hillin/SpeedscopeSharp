using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hillinworks.Speedcore.FileFormat
{
    internal abstract class FrameEventBase : IEvent
    {
        protected FrameEventBase(double at, int frame)
        {
            this.At = at;
            this.Frame = frame;
        }

        [JsonProperty("frame")]
        public int Frame { get; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public abstract EventType Type { get; }

        [JsonProperty("at")]
        public double At { get; }
    }
}