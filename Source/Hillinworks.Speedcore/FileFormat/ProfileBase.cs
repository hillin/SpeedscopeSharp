using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hillinworks.Speedcore.FileFormat
{
    internal abstract class ProfileBase : IProfile
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public abstract ProfileType Type { get; }
        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("unit")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ValueUnit Unit { get; }
        [JsonProperty("startValue")]
        public double StartValue { get; }
        [JsonProperty("endValue")]
        public double EndValue { get; }

        protected ProfileBase(string name, double startValue, double endValue, ValueUnit unit)
        {
            this.Name = name;
            this.StartValue = startValue;
            this.EndValue = endValue;
            this.Unit = unit;
        }
    }
}