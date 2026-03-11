using System.Collections.Generic;
using Features.FiniteStateMachine.Interfaces;
using Features.GameSystem.Interfaces.Handlers;

namespace Features.GameStateMachine.States
{
    public class InitState : IState
    {
        private readonly IMachine                               _stateMachine;
        private readonly IReadOnlyList<IStartableSystemHandler> _startableSystemHandlers;

        public InitState(IMachine stateMachine, IReadOnlyList<IStartableSystemHandler> startableSystemHandlers)
        {
            _stateMachine            = stateMachine;
            _startableSystemHandlers = startableSystemHandlers;
        }

        public void Enter()
        {
            for (var i = 0; i < _startableSystemHandlers.Count; i++) _startableSystemHandlers[i].Initialize();

            _stateMachine.Enter<GameloopState>();
        }

        public void Exit()
        {
        }
    }
}
