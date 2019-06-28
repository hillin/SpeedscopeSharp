namespace Hillinworks.Speedscope.FileFormat
{
    internal interface IEvent
    {
        EventType Type { get; }
        double At { get; }
    }
}