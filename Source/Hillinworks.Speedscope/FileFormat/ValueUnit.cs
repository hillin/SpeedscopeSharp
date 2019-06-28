using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Hillinworks.Speedscope.FileFormat
{
    internal enum ValueUnit
    {
        [EnumMember(Value ="none")]
        None,
        [EnumMember(Value ="bytes")]
        Bytes,
        [EnumMember(Value ="nanoseconds")]
        Nanoseconds,
        [EnumMember(Value ="microseconds")]
        Microseconds,
        [EnumMember(Value ="milliseconds")]
        Milliseconds,
        [EnumMember(Value ="seconds")]
        Seconds
    }
}