using Features.Input.PointerStates;
using VContainer;
using VContainer.Unity;

namespace Features.Input
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
