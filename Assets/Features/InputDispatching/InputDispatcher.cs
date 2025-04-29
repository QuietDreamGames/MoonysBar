using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.InputDispatching
{
    public class InputDispatcher : MonoBehaviour
    {
        public event Action<InputAction.CallbackContext>       OnClickAction;
        public event Action<InputAction.CallbackContext, bool> OnHoldClickAction;
        
        public void OnClick(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            OnClickAction?.Invoke(context);
            Debug.Log("Click performed");
        }
        
        public void OnClickHold(InputAction.CallbackContext context) 
        {
            if (context.performed) return;
            if (context.started)
            {
                Debug.Log("Click hold started");
                OnHoldClickAction?.Invoke(context, true);
            }
            else if (context.canceled)
            {
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