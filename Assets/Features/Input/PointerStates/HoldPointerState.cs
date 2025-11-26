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

        private Vector2 _totalMoveDelta;

        public HoldPointerState(
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
            _totalMoveDelta                         =  Vector2.zero;
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
            _stateMachine.Enter<IdlePointerState>();
            _inputEventBus.PointerHoldEndFire();
        }

        private void OnPointerMove(InputAction.CallbackContext context, Vector2 delta)
        {
            _totalMoveDelta += delta;
            if (_totalMoveDelta.magnitude < 1.5) return;

            _stateMachine.Enter<DragPointerState>();
            _inputEventBus.PointerDragFire(delta);
        }
    }
}
