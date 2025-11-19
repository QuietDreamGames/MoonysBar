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

        public void OnPointerPress(InputAction.CallbackContext context)
        {
            if (context.performed) return;
            if (context.started)
            {
                if (isDebugMode)
                    Debug.Log("Click hold started");

                OnPointerPressAction?.Invoke(context);
            }
            else if (context.canceled)
            {
                if (isDebugMode)
                    Debug.Log("Click hold canceled");

                OnPointerReleaseAction?.Invoke(context);
            }
        }

        public void OnPointerMove(InputAction.CallbackContext context)
        {
            if (isDebugMode)
                Debug.Log("Pointer moved");

            var delta = context.ReadValue<Vector2>();

            OnPointerMoveAction?.Invoke(arg1: context, arg2: delta);
        }
    }
}
