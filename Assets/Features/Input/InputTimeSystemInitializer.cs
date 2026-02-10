using Features.TimeSystem.Interfaces;
using VContainer;
using VContainer.Unity;

namespace Features.Input
{
    public class InputTimeSystemInitializer : IStartable
    {
        [Inject] private readonly ITimeSystem     _timeSystem;
        [Inject] private readonly IUpdateProvider _updateProvider;

        public void Start()
        {
            _timeSystem.SetUpdateProvider(_updateProvider);
            _timeSystem.Initialize();
        }
    }
}
