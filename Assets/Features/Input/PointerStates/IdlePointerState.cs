using Features.FiniteStateMachine.Interfaces;
using Features.Input.Interfaces;
using UnityEngine.InputSystem;

namespace Features.Input.PointerStates
{
    public class IdlePointerState : IState
    {
        private readonly IInputDispatcher   _inputDispatcher;
        private readonly IInputEventBusSink _inputEventBus;
        private readonly IMachine           _stateMachine;

        public IdlePointerState(
            IMachine           stateMachine,
            IInputDispatcher   inputDispatcher,
            IInputEventBusSink inputEventBus
        )
        {
            _stateMachine    = stateMachine;
            _inputDispatcher = inputDispatcher;
            _inputEventBus   = inputEventBus;
        }

        public void Enter()
        {
            _inputDispatcher.OnPointerPressAction += OnPointerPressed;
        }

        public void Exit()
        {
            _inputDispatcher.OnPointerPressAction -= OnPointerPressed;
        }

        private void OnPointerPressed(InputAction.CallbackContext context)
        {
            _stateMachine.Enter<HoldPointerState>();
            _inputEventBus.PointerHoldStartFire();
        }
    }
}
