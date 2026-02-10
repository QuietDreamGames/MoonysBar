using Features.FiniteStateMachine.Interfaces;
using Features.Input.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Input.PointerStates
{
    public class HoldPointerState : IState
    {
        private readonly IMachine           _stateMachine;
        private readonly IInputDispatcher   _inputDispatcher;
        private readonly IInputEventBusSink _inputEventBus;
        private readonly IInputClickTimer   _clickTimer;

        public HoldPointerState(
            IMachine           stateMachine,
            IInputDispatcher   inputDispatcher,
            IInputEventBusSink inputEventBus,
            IInputClickTimer   clickTimer
        )
        {
            _stateMachine    = stateMachine;
            _inputDispatcher = inputDispatcher;
            _inputEventBus   = inputEventBus;
            _clickTimer      = clickTimer;
        }

        public void Enter()
        {
            _inputDispatcher.OnPointerReleaseAction += OnPointerRelease;
            _inputDispatcher.OnPointerMoveAction    += OnPointerMove;

            _clickTimer.Restart();
        }

        public void Exit()
        {
            _inputDispatcher.OnPointerReleaseAction -= OnPointerRelease;
            _inputDispatcher.OnPointerMoveAction    -= OnPointerMove;
        }

        private void OnPointerRelease(InputAction.CallbackContext context)
        {
            var isAboveClickThreshold = _clickTimer.IsTimerAboveClickThreshold();
            _clickTimer.Stop();
            _stateMachine.Enter<IdlePointerState>();
            _inputEventBus.PointerHoldEndFire();
            if (!isAboveClickThreshold)
                _inputEventBus.PointerClickFire();
        }

        private void OnPointerMove(InputAction.CallbackContext context, Vector2 delta)
        {
            _stateMachine.Enter<DragPointerState>();
            _inputEventBus.PointerDragFire(delta);
        }
    }
}
