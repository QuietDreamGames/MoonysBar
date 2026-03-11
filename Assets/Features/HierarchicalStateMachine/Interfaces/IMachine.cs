namespace Features.HierarchicalStateMachine.Interfaces
{
    public interface IMachine
    {
        void Init();
        void Update();

        bool TryHandleEvent<T>(out bool isHandled) where T : IEvent;
        void EnterState<T>() where T : IState;
    }
}
