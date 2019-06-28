using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hillinworks.Speedcore.FileFormat
{
    internal class Profile
    {
        [JsonConstructor]
        public Profile(SharedData shared, List<IProfile> profiles)
        {
            this.Shared = shared;
            this.Profiles = profiles;
        }

        public Profile()
            : this(new SharedData(), new List<IProfile>())
        {
        }

        [JsonProperty("$schema")]
        public string Schema => "https://www.speedscope.app/file-format-schema.json";

        [JsonProperty("shared")]
        public SharedData Shared { get; }

        [JsonProperty("profiles")]
        public List<IProfile> Profiles { get; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("activeProfileIndex")]
        public int? ActiveProfileIndex { get; set; }

        [JsonProperty("exporter")]
        public string? Exporter { get; set; }
    }
}