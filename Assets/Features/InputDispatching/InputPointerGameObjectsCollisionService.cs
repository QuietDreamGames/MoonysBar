using System;
using Features.CameraSystem;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Features.InputDispatching
{
    public class InputPointerGameObjectsCollisionService
    {
        private readonly CameraHolderService _cameraHolderService;

        private GameObject _heldGameObject;

        public event Action<GameObject>       OnClickedGameObjectAction;
        public event Action<GameObject, bool> OnHeldGameObjectAction;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public InputPointerGameObjectsCollisionService(
            CameraHolderService cameraHolderService,
            InputService        inputService)
        {
            _cameraHolderService           =  cameraHolderService;
            inputService.OnClickAction     += CheckClickedCollisionWithObjects;
            inputService.OnHoldClickAction += CheckHeldCollisionWithObjects;
        }


        private void CheckClickedCollisionWithObjects(InputAction.CallbackContext context)
        {
            var rayHit = Physics2D.GetRayIntersection(
                _cameraHolderService.MainCamera.ScreenPointToRay(InputUtils.GetPrimaryPointerScreenPosition()));
            if (!rayHit.collider) return;

            OnClickedGameObjectAction?.Invoke(rayHit.collider.gameObject);
        }

        private void CheckHeldCollisionWithObjects(InputAction.CallbackContext context, bool isStarted)
        {
            if (context.canceled)
            {
                OnHeldGameObjectAction?.Invoke(_heldGameObject, false);
                _heldGameObject = null;
                return;
            }


            var rayHit = Physics2D.GetRayIntersection(
                _cameraHolderService.MainCamera.ScreenPointToRay(InputUtils.GetPrimaryPointerScreenPosition()));

            if (!rayHit.collider)
            {
                if (_heldGameObject == null) return;
                OnHeldGameObjectAction?.Invoke(_heldGameObject, false);
                _heldGameObject = null;
                return;
            }

            _heldGameObject = rayHit.collider.gameObject;
            OnHeldGameObjectAction?.Invoke(_heldGameObject, true);
        }
    }
}