using Features.FiniteStateMachine.Interfaces;
using Features.Input.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Input.PointerStates
{
    public class DragPointerState : IState
    {
        private readonly IInputDispatcher   _inputDispatcher;
        private readonly IMachine           _stateMachine;
        private readonly IInputEventBusSink _inputEventBus;
        private readonly IInputClickTimer   _clickTimer;

        public DragPointerState(
            IMachine           stateMachine,
            IInputDispatcher   inputDispatcher,
            IInputEventBusSink inputEventBus,
            IInputClickTimer   clickTimer
        )
        {
            _inputDispatcher = inputDispatcher;
            _stateMachine    = stateMachine;
            _inputEventBus   = inputEventBus;
            _clickTimer      = clickTimer;
        }

        public void Enter()
        {
            _inputDispatcher.OnPointerReleaseAction += OnPointerRelease;
            _inputDispatcher.OnPointerMoveAction    += OnPointerMove;
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
            _inputEventBus.PointerDragEndFire();
            if (!isAboveClickThreshold)
                _inputEventBus.PointerClickFire();
        }

        private void OnPointerMove(InputAction.CallbackContext context, Vector2 delta)
        {
            _inputEventBus.PointerDragFire(delta);
        }
    }
}
