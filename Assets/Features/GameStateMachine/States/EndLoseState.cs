using Features.GameSystem.Interfaces.Handlers;

namespace Features.GameStateMachine.States
{
    public class EndLoseState : EndState
    {
        public EndLoseState(params IEndableSystemHandler[] endableSystemHandlers) : base(endableSystemHandlers)
        {
        }
    }
}