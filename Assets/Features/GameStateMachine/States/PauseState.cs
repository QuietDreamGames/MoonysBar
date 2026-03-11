using System.Collections.Generic;
using Features.FiniteStateMachine.Interfaces;
using Features.GameSystem.Interfaces.Handlers;

namespace Features.GameStateMachine.States
{
    public class PauseState : IState
    {
        private readonly IReadOnlyList<IPausableSystemHandler> _pausableSystemHandlers;

        public PauseState(IReadOnlyList<IPausableSystemHandler> pausableSystemHandlers)
        {
            _pausableSystemHandlers = pausableSystemHandlers;
        }

        public void Enter()
        {
            for (var i = 0; i < _pausableSystemHandlers.Count; i++) _pausableSystemHandlers[i].Pause();
        }

        public void Exit()
        {
        }
    }
}
