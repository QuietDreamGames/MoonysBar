using Features.InputDispatching.PointerStates;
using VContainer;
using VContainer.Unity;

namespace Features.InputDispatching
{
    public class InputServiceEntry : IStartable
    {
        [Inject] private InputPointerStateMachine _inputPointerStateMachine;

        public void Start()
        {
            _inputPointerStateMachine.Enter<IdlePointerState>();
        }
    }
}
