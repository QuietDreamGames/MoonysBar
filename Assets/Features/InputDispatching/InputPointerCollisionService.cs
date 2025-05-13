using System;
using Features.CameraSystem;
using Features.Collision;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Features.InputDispatching
{
    public class InputPointerCollisionService
    {
        private readonly CameraHolderService _cameraHolderService;

        private PointerCollider _heldPointerCollider;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public InputPointerCollisionService(
            CameraHolderService cameraHolderService,
            InputService        inputService)
        {
            _cameraHolderService           =  cameraHolderService;
            inputService.OnClickAction     += CheckClickedCollisionWithObjects;
            inputService.OnHoldClickAction += CheckHeldCollisionWithObjects;
        }

        public event Action<PointerCollider>       OnClickedPointerColliderAction;
        public event Action<PointerCollider, bool> OnHeldPointerColliderAction;

        private void CheckClickedCollisionWithObjects(InputAction.CallbackContext context)
        {
            var rayHit = Physics2D.GetRayIntersection(
                _cameraHolderService.MainCamera.ScreenPointToRay(InputUtils.GetPrimaryPointerScreenPosition()));
            if (!rayHit.collider) return;

            if (rayHit.collider.TryGetComponent(out PointerCollider pointerCollider))
                OnClickedPointerColliderAction?.Invoke(pointerCollider);
        }

        private void CheckHeldCollisionWithObjects(InputAction.CallbackContext context, bool isStarted)
        {
            if (context.canceled)
            {
                if (!_heldPointerCollider) return;
                OnHeldPointerColliderAction?.Invoke(_heldPointerCollider, false);
                _heldPointerCollider = null;
                return;
            }

            var rayHit = Physics2D.GetRayIntersection(
                _cameraHolderService.MainCamera.ScreenPointToRay(InputUtils.GetPrimaryPointerScreenPosition()));

            if (!rayHit.collider)
            {
                if (!_heldPointerCollider) return;
                OnHeldPointerColliderAction?.Invoke(_heldPointerCollider, false);
                _heldPointerCollider = null;
                return;
            }

            if (!rayHit.collider.TryGetComponent(out PointerCollider pointerCollider)) return;

            _heldPointerCollider = pointerCollider;
            OnHeldPointerColliderAction?.Invoke(pointerCollider, true);
        }
    }
}
