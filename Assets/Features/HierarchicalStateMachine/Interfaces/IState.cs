namespace Features.HierarchicalStateMachine.Interfaces
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();

        bool TryHandleEvent<T>(out bool isHandled) where T : IEvent;
    }
}
