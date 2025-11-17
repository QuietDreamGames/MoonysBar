using System;
using Features.InputDispatching.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.InputDispatching
{
    public class InputDispatcher : MonoBehaviour, IInputDispatcher, IInputSchemeSelector
    {
        [SerializeField] private bool isDebugMode = false;

        private void OnDestroy()
        {
            OnPointerPressAction   = null;
            OnPointerReleaseAction = null;
            OnPointerMoveAction    = null;
        }

        public event Action<InputAction.CallbackContext>          OnPointerPressAction;
        public event Action<InputAction.CallbackContext>          OnPointerReleaseAction;
        public event Action<InputAction.CallbackContext, Vector2> OnPointerMoveAction;

        public void SetInputScheme(InputSchemeType inputSchemeType)
        {
            if (isDebugMode)
                Debug.LogWarning($"NOT IMPLEMENTED. Tried to set input scheme to: {inputSchemeType}");
        }

        public void OnPointerPress(InputAction.CallbackContext context, bool isPressed)
        {
            if (isDebugMode)
                Debug.Log(isPressed ? "Pointer pressed" : "Pointer released");

            if (isPressed)
                OnPointerPressAction?.Invoke(context);
            else
                OnPointerReleaseAction?.Invoke(context);
        }

        public void OnPointerMove(InputAction.CallbackContext context, Vector2 delta)
        {
            if (isDebugMode)
                Debug.Log("Pointer moved");

            OnPointerMoveAction?.Invoke(arg1: context, arg2: delta);
        }
    }
}
