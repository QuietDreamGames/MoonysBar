using Features.FiniteStateMachine.Interfaces;
using Features.Input.Interfaces;
using Features.TimeSystem.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Input.PointerStates
{
    public class PendingPointerState : IState, IUpdateHandler
    {
        private readonly IMachine           _stateMachine;
        private readonly IInputDispatcher   _inputDispatcher;
        private readonly IInputEventBusSink _inputEventBus;

        private       float   _elapsedTime;
        private       bool    _isActive;
        private       Vector2 _totalMoveDelta;
        private const float   PendingDuration = 0.2f;


        public PendingPointerState(
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
            _isActive       = true;
            _elapsedTime    = 0f;
            _totalMoveDelta = Vector2.zero;

            _inputDispatcher.OnPointerReleaseAction += OnPointerRelease;
            _inputDispatcher.OnPointerMoveAction    += OnPointerMove;
        }

        public void Exit()
        {
            _isActive = false;

            _inputDispatcher.OnPointerReleaseAction -= OnPointerRelease;
            _inputDispatcher.OnPointerMoveAction    -= OnPointerMove;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_isActive) return;

            _elapsedTime += deltaTime;
            if (_elapsedTime >= PendingDuration)
            {
                _stateMachine.Enter<HoldPointerState>();
                _inputEventBus.PointerHoldStartFire();
            }
        }

        private void OnPointerRelease(InputAction.CallbackContext context)
        {
            _stateMachine.Enter<IdlePointerState>();
            _inputEventBus.PointerClickFire();
        }

        private void OnPointerMove(InputAction.CallbackContext context, Vector2 delta)
        {
            _totalMoveDelta += delta;
            if (_totalMoveDelta.magnitude < 1.5f) return;

            _stateMachine.Enter<DragPointerState>();
            _inputEventBus.PointerDragFire(delta);
        }
    }
}
