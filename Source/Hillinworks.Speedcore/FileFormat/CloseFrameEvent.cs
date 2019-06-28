namespace Hillinworks.Speedcore.FileFormat
{
    internal class CloseFrameEvent : FrameEventBase
    {
        public CloseFrameEvent(double at, int frame)
            : base(at, frame)
        {
        }

        public override EventType Type => EventType.CloseFrame;
    }
}