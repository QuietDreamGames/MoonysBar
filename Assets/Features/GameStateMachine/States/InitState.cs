using Features.FiniteStateMachine.Interfaces;
using Features.GameSystem.Interfaces.Handlers;

namespace Features.GameStateMachine.States
{
    public class InitState : IState
    {
        private readonly IMachine                  _stateMachine;
        private readonly IStartableSystemHandler[] _startableSystemHandlers;

        public InitState(IMachine stateMachine, params IStartableSystemHandler[] startableSystemHandlers)
        {
            _stateMachine            = stateMachine;
            _startableSystemHandlers = startableSystemHandlers;
        }

        public void Enter()
        {
            foreach (var initable in _startableSystemHandlers)
            {
                initable.Initialize();
            }

            _stateMachine.Enter<GameloopState>();
        }

        public void Exit()
        {
        }
    }
}