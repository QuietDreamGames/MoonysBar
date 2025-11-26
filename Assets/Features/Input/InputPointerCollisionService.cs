using System;
using System.Collections.Generic;
using Features.CameraSystem;
using Features.Collision;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Features.Input
{
    public class InputPointerCollisionService : ITickable
    {
        private readonly CameraHolderService   _cameraHolderService;
        private readonly List<PointerCollider> _hoveredPointerColliders = new();

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
            HandleHeldCollisions();
            HandleHoveredCollisions();
        }

        private void HandleHeldCollisions()
        {
            if (!_isHoldingPointerCollider) return;

            var rayHit = Physics2D.GetRayIntersection(
                _cameraHolderService.MainCamera.ScreenPointToRay(InputUtils.GetPrimaryPointerScreenPosition()));

            if (!rayHit.collider)
            {
                OnHeldPointerColliderAction?.Invoke(arg1: _heldPointerCollider, arg2: false);
                _heldPointerCollider      = null;
                _isHoldingPointerCollider = false;
                return;
            }

            if (rayHit.collider == _heldPointerCollider.Collider) return;

            OnHeldPointerColliderAction?.Invoke(arg1: _heldPointerCollider, arg2: false);
            _heldPointerCollider      = null;
            _isHoldingPointerCollider = false;
        }

        private void HandleHoveredCollisions()
        {
            if (OnHoveredPointerColliderAction != null &&
                OnHoveredPointerColliderAction.GetInvocationList().Length > 0) return;

            var rayHits = Physics2D.GetRayIntersectionAll(
                _cameraHolderService.MainCamera.ScreenPointToRay(InputUtils.GetPrimaryPointerScreenPosition())
            );

            var currentlyHoveredPointerColliders = new List<PointerCollider>();

            for (var i = 0; i < rayHits.Length; i++)
                if (rayHits[i].collider.TryGetComponent(out PointerCollider pointerCollider))
                {
                    currentlyHoveredPointerColliders.Add(pointerCollider);
                    if (_hoveredPointerColliders.Contains(pointerCollider)) continue;
                    OnHoveredPointerColliderAction?.Invoke(arg1: pointerCollider, arg2: true);
                }

            for (var i = 0; i < _hoveredPointerColliders.Count; i++)
                if (!currentlyHoveredPointerColliders.Contains(_hoveredPointerColliders[i]))
                    OnHoveredPointerColliderAction?.Invoke(arg1: _hoveredPointerColliders[i], arg2: false);
        }

        public event Action<PointerCollider>       OnClickedPointerColliderAction;
        public event Action<PointerCollider, bool> OnHeldPointerColliderAction;
        public event Action<PointerCollider, bool> OnHoveredPointerColliderAction;

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
                OnHeldPointerColliderAction?.Invoke(arg1: _heldPointerCollider, arg2: false);
                _heldPointerCollider      = null;
                _isHoldingPointerCollider = false;
                return;
            }

            var rayHit = Physics2D.GetRayIntersection(
                _cameraHolderService.MainCamera.ScreenPointToRay(InputUtils.GetPrimaryPointerScreenPosition()));

            if (!rayHit.collider)
            {
                if (!_isHoldingPointerCollider) return;
                OnHeldPointerColliderAction?.Invoke(arg1: _heldPointerCollider, arg2: false);
                _heldPointerCollider      = null;
                _isHoldingPointerCollider = false;
                return;
            }

            if (!rayHit.collider.TryGetComponent(out PointerCollider pointerCollider)) return;

            _heldPointerCollider      = pointerCollider;
            _isHoldingPointerCollider = true;
            OnHeldPointerColliderAction?.Invoke(arg1: pointerCollider, arg2: true);
        }
    }
}
