namespace Features.GameSystem.Interfaces.Handlers
{
    public interface IPausableSystemHandler : ISystem
    {
        void Pause();
        void Resume();
    }
}