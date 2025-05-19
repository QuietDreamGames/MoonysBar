using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.InputDispatching
{
    public class InputDispatcher : MonoBehaviour
    {
        [SerializeField] private bool isDebugMode = false;

        public event Action<InputAction.CallbackContext>       OnClickAction;
        public event Action<InputAction.CallbackContext, bool> OnHoldClickAction;

        public void OnClick(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (isDebugMode)
                Debug.Log("Click performed");

            OnClickAction?.Invoke(context);
        }

        public void OnClickHold(InputAction.CallbackContext context)
        {
            if (context.performed) return;
            if (context.started)
            {
                if (isDebugMode)
                    Debug.Log("Click hold started");

                OnHoldClickAction?.Invoke(context, true);
            }
            else if (context.canceled)
            {
                if (isDebugMode)
                    Debug.Log("Click hold canceled");

                OnHoldClickAction?.Invoke(context, false);
            }
        }

        private void OnDestroy()
        {
            OnClickAction     = null;
            OnHoldClickAction = null;
        }
    }
}
