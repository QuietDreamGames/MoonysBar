using Features.FiniteStateMachine.Interfaces;
using Features.GameSystem.Interfaces.Handlers;

namespace Features.GameStateMachine.States
{
    public abstract class EndState : IState
    {
        private readonly IEndableSystemHandler[] _endableSystemHandlers;

        public EndState(IEndableSystemHandler[] endableSystemHandlers)
        {
            _endableSystemHandlers = endableSystemHandlers;
        }

        public virtual void Enter()
        {
            foreach (var endableSystemHandler in _endableSystemHandlers)
            {
                endableSystemHandler.Terminate();
            }
        }

        public virtual void Exit()
        {
        }
    }
}