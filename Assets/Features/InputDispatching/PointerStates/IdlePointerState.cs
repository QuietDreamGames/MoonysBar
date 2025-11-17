using Features.FiniteStateMachine.Interfaces;
using Features.InputDispatching.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.InputDispatching.PointerStates
{
    public class IdlePointerState : IState
    {
        private readonly IMachine         _stateMachine;
        private readonly IInputDispatcher _inputSystem;


        public IdlePointerState(IMachine stateMachine, IInputDispatcher inputSystem)
        {
            _stateMachine = stateMachine;
            _inputSystem  = inputSystem;
        }

        public void Enter()
        {
            _inputSystem.OnPointerPressAction += OnPointerPressed;
            // _inputSystem.OnPointerMoveAction  += OnPointerMoved;
        }

        public void Exit()
        {
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
