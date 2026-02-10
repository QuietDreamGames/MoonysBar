using Features.Input.Interfaces;
using Features.Parameters;
using Features.TimeSystem.Interfaces;
using Features.TimeSystem.Interfaces.Handlers;
using Features.TimeSystem.Interfaces.Injected;
using VContainer;
using VContainer.Unity;

namespace Features.Input
{
    public class InputClickTimer : IInputClickTimer, IStartable, IUpdateHandler
    {
        [Inject] private DefaultRootParametersHolder _rootParametersHolder;
        [Inject] private ITimeSystem                 _timeSystem;
        [Inject] private ITransientTimeCollector     _transientTimeCollector;

        private float _clickTimer;

        private bool _isTimerRunning;

        public void Start()
        {
            _transientTimeCollector.UpdateHandlers.Add(this);
            _timeSystem.Subscribe(_transientTimeCollector);
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_isTimerRunning) return;
            _clickTimer += deltaTime;
        }

        public void Restart()
        {
            _clickTimer     = 0f;
            _isTimerRunning = true;
        }

        public void Stop()
        {
            _isTimerRunning = false;
        }

        public bool IsTimerAboveClickThreshold()
        {
            return _clickTimer >= _rootParametersHolder.InputParameters.PointerClickThreshold;
        }
    }
}
