namespace Hillinworks.Speedcore.FileFormat
{
    internal interface IEvent
    {
        EventType Type { get; }
        double At { get; }
    }
}