using System;
using Features.CameraSystem;
using Features.Collision;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Features.InputDispatching
{
    public class InputPointerCollisionService : ITickable
    {
        private readonly CameraHolderService _cameraHolderService;

        private PointerCollider _heldPointerCollider;
        private bool            _isHoldingPointerCollider;

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

        public void Tick()
        {
            if (!_isHoldingPointerCollider) return;

            var rayHit = Physics2D.GetRayIntersection(
                _cameraHolderService.MainCamera.ScreenPointToRay(InputUtils.GetPrimaryPointerScreenPosition()));

            if (!rayHit.collider)
            {
                OnHeldPointerColliderAction?.Invoke(_heldPointerCollider, false);
                _heldPointerCollider      = null;
                _isHoldingPointerCollider = false;
                return;
            }

            if (rayHit.collider == _heldPointerCollider.Collider) return;

            OnHeldPointerColliderAction?.Invoke(_heldPointerCollider, false);
            _heldPointerCollider      = null;
            _isHoldingPointerCollider = false;
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
                if (!_isHoldingPointerCollider) return;
                OnHeldPointerColliderAction?.Invoke(_heldPointerCollider, false);
                _heldPointerCollider      = null;
                _isHoldingPointerCollider = false;
                return;
            }

            var rayHit = Physics2D.GetRayIntersection(
                _cameraHolderService.MainCamera.ScreenPointToRay(InputUtils.GetPrimaryPointerScreenPosition()));

            if (!rayHit.collider)
            {
                if (!_isHoldingPointerCollider) return;
                OnHeldPointerColliderAction?.Invoke(_heldPointerCollider, false);
                _heldPointerCollider      = null;
                _isHoldingPointerCollider = false;
                return;
            }

            if (!rayHit.collider.TryGetComponent(out PointerCollider pointerCollider)) return;

            _heldPointerCollider      = pointerCollider;
            _isHoldingPointerCollider = true;
            OnHeldPointerColliderAction?.Invoke(pointerCollider, true);
        }
    }
}
