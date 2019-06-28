using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hillinworks.Speedscope.FileFormat
{
    internal class SharedData
    {
        [JsonProperty("frames")]
        public List<Frame> Frames { get; }

        [JsonConstructor]
        public SharedData(List<Frame> frames)
        {
            this.Frames = frames;
        }

        public SharedData()
            : this(new List<Frame>())
        {
        }
    }
}