using System;
using JetBrains.Annotations;
using UnityEngine.InputSystem;
using VContainer;

namespace Features.InputDispatching
{
    public class InputService
    {
        public Action<InputAction.CallbackContext>       OnClickAction;
        public Action<InputAction.CallbackContext, bool> OnHoldClickAction;
        
        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public InputService(InputDispatcher inputDispatcher)
        {
            inputDispatcher.OnClickAction     += OnClick;
            inputDispatcher.OnHoldClickAction += OnHoldClick;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            OnClickAction?.Invoke(context);
        }
        
        private void OnHoldClick(InputAction.CallbackContext context, bool isStarted)
        {
            OnHoldClickAction?.Invoke(context, isStarted);
        }
    }
}