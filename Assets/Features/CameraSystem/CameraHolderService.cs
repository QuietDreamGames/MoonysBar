using Features.Interaction;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.CameraSystem
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class CameraHolderService : IStartable
    {
        [Inject] private readonly InteractionCameraHolder _interactionCameraHolder;
        [Inject] private          Camera                  _mainCamera;

        public Camera MainCamera => _mainCamera;

        public void Start()
        {
            if (_mainCamera == null) Debug.LogWarning("CameraHolderService was initialized with no main camera");
            _interactionCameraHolder.SetInteractionCamera(MainCamera);
        }

        public void ChangeMainCamera(Camera camera)
        {
            _interactionCameraHolder.SetInteractionCamera(MainCamera);
            _mainCamera = camera;
        }
    }
}
