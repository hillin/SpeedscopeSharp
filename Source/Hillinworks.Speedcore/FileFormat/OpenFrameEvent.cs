namespace Hillinworks.Speedcore.FileFormat
{
    internal class OpenFrameEvent : FrameEventBase
    {
        public OpenFrameEvent(double at, int frame)
            : base(at, frame)
        {
        }

        public override EventType Type => EventType.OpenFrame;
    }
}