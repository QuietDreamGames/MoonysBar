using Features.TimeSystem.Interfaces;
using VContainer;
using VContainer.Unity;

namespace Features.Input
{
    public class InputTimeSystemInitializer : IStartable
    {
        [Inject] private readonly ITimeSystem _timeSystem;

        public void Start()
        {
            _timeSystem.Initialize();
        }
    }
}
