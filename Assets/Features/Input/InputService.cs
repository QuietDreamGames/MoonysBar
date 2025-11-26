using System;
using JetBrains.Annotations;
using UnityEngine.InputSystem;
using VContainer;

namespace Features.Input
{
    public class InputService
    {
        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public InputService(InputDispatcher inputDispatcher)
        {
            // inputDispatcher.OnClickAction     += OnClick;
            // inputDispatcher.OnHoldClickAction += OnHoldClick;
        }

        public event Action<InputAction.CallbackContext>       OnClickAction;
        public event Action<InputAction.CallbackContext, bool> OnHoldClickAction;

        private void OnClick(InputAction.CallbackContext context)
        {
            OnClickAction?.Invoke(context);
        }

        private void OnHoldClick(InputAction.CallbackContext context, bool isStarted)
        {
            OnHoldClickAction?.Invoke(arg1: context, arg2: isStarted);
        }
    }
}
