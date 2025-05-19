using UnityEngine;
using VContainer;
using JetBrains.Annotations;

namespace Features.CameraSystem
{
    public class CameraHolderService
    {
        private Camera _mainCamera;

        public Camera MainCamera => _mainCamera;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public CameraHolderService(Camera mainCamera)
        {
            _mainCamera = mainCamera;

            if (_mainCamera == null)
            {
                Debug.LogWarning("CameraHolderService was initialized with no main camera");
            }
        }

        public void ChangeMainCamera(Camera camera)
        {
            _mainCamera = camera;
        }
    }
}