using System.Runtime.Serialization;

namespace Hillinworks.Speedcore.FileFormat
{
    public enum EventType
    {
        [EnumMember(Value = "O")]
        OpenFrame,
        [EnumMember(Value = "C")]
        CloseFrame
    }
}