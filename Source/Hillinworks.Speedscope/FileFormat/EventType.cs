using System.Runtime.Serialization;

namespace Hillinworks.Speedscope.FileFormat
{
    public enum EventType
    {
        [EnumMember(Value = "O")]
        OpenFrame,
        [EnumMember(Value = "C")]
        CloseFrame
    }
}