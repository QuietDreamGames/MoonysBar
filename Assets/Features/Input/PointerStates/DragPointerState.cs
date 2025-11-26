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

        public DragPointerState(
            IMachine           stateMachine,
            IInputDispatcher   inputDispatcher,
            IInputEventBusSink inputEventBus
        )
        {
            _inputDispatcher = inputDispatcher;
            _stateMachine    = stateMachine;
            _inputEventBus   = inputEventBus;
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
            _stateMachine.Enter<IdlePointerState>();
            _inputEventBus.PointerDragEndFire();
        }

        private void OnPointerMove(InputAction.CallbackContext context, Vector2 delta)
        {
            _inputEventBus.PointerDragFire(delta);
        }
    }
}
