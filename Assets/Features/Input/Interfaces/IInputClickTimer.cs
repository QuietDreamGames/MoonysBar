namespace Features.Input.Interfaces
{
    public interface IInputClickTimer
    {
        void Restart();
        void Stop();
        bool IsTimerAboveClickThreshold();
    }
}
