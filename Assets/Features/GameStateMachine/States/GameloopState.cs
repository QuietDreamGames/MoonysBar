using Features.FiniteStateMachine.Interfaces;
using Features.GameSystem.Interfaces.Handlers;

namespace Features.GameStateMachine.States
{
    public class GameloopState : IState
    {
        private readonly IPausableSystemHandler[] _pausableSystemHandlers;

        public GameloopState(params IPausableSystemHandler[] pausableSystemHandlers)
        {
            _pausableSystemHandlers = pausableSystemHandlers;
        }

        public void Enter()
        {
            // TODO: gameplay time system must be unpaused here

            foreach (var pausableSystemHandler in _pausableSystemHandlers)
            {
                pausableSystemHandler.Resume();
            }
        }

        public void Exit()
        {
        }
    }
}