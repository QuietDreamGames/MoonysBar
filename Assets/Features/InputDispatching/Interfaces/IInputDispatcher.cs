using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.InputDispatching.Interfaces
{
    public interface IInputDispatcher
    {
        public event Action<InputAction.CallbackContext> OnPointerPressAction;
        public event Action<InputAction.CallbackContext> OnPointerReleaseAction;

        public event Action<InputAction.CallbackContext, Vector2> OnPointerMoveAction;
    }
}
