using Features.FiniteStateMachine.Interfaces;
using Features.InputDispatching.Interfaces;

namespace Features.InputDispatching.PointerStates
{
    public class ClickPointerState : IState
    {
        private readonly IMachine         _stateMachine;
        private readonly IInputDispatcher _inputSystem;

        public ClickPointerState(IMachine stateMachine, IInputDispatcher inputSystem)
        {
            _stateMachine = stateMachine;
            _inputSystem  = inputSystem;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}
