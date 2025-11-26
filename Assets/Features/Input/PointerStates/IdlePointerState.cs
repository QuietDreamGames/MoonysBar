using Features.FiniteStateMachine.Interfaces;
using Features.Input.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Input.PointerStates
{
    public class IdlePointerState : IState
    {
        private readonly IMachine         _stateMachine;
        private readonly IInputDispatcher _inputSystem;

        private Vector2 _totalMoveDelta;


        public IdlePointerState(IMachine stateMachine, IInputDispatcher inputSystem)
        {
            _stateMachine = stateMachine;
            _inputSystem  = inputSystem;
        }

        public void Enter()
        {
            _totalMoveDelta                   =  Vector2.zero;
            _inputSystem.OnPointerPressAction += OnPointerPressed;
            // _inputSystem.OnPointerMoveAction  += OnPointerMoved;
        }

        public void Exit()
        {
            _totalMoveDelta                   =  Vector2.zero;
            _inputSystem.OnPointerPressAction -= OnPointerPressed;
            // _inputSystem.OnPointerMoveAction  -= OnPointerMoved;
        }

        private void OnPointerPressed(InputAction.CallbackContext context)
        {
            _stateMachine.Enter<PendingPointerState>();
        }

        private void OnPointerMoved(InputAction.CallbackContext context, Vector2 delta)
        {
            // _stateMachine.Enter<MovePointerState>();
        }
    }
}
