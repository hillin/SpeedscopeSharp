using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Hillinworks.Speedcore.FileFormat
{
    public enum ProfileType
    {
        [EnumMember(Value = "evented")]
        Evented,
        [EnumMember(Value = "sampled")]
        Sampled
    }
}