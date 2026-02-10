using Features.TimeSystem.Interfaces;
using VContainer;
using VContainer.Unity;

namespace Features.Interaction
{
    public class InteractionTimeSystemInitializer : IStartable
    {
        [Inject] private readonly ITimeSystem _timeSystem;

        public void Start()
        {
            _timeSystem.Initialize();
        }
    }
}
