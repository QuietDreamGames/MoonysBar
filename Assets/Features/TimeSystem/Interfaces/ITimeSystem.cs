using Features.GameSystem.Interfaces.Handlers;

namespace Features.TimeSystem.Interfaces
{
    public interface ITimeSystem : IStartableSystemHandler, IPausableSystemHandler
    {
        void SetUpdateProvider(IUpdateProvider updateProvider);

        void Subscribe(ITimeCollector timeCollector);
        void Unsubscribe(ITimeCollector timeCollector);

        float GetTimeScale();
        void  SetTimeScale(float timeScale);
    }
}