using System.Collections.Generic;
using Features.FiniteStateMachine.Interfaces;
using Features.GameSystem.Interfaces.Handlers;

namespace Features.GameStateMachine.States
{
    public class EndState : IState
    {
        private readonly IReadOnlyList<IEndableSystemHandler> _endableSystemHandlers;

        public EndState(IReadOnlyList<IEndableSystemHandler> endableSystemHandlers)
        {
            _endableSystemHandlers = endableSystemHandlers;
        }

        public void Enter()
        {
            for (var i = 0; i < _endableSystemHandlers.Count; i++) _endableSystemHandlers[i].Terminate();
        }

        public void Exit()
        {
        }
    }
}
