using Features.GameSystem.Interfaces.Handlers;

namespace Features.GameStateMachine.States
{
    public class EndWinState : EndState
    {
        public EndWinState(params IEndableSystemHandler[] endableSystemHandlers) : base(endableSystemHandlers)
        {
        }
    }
}