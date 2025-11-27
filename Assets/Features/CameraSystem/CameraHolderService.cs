using Features.Interaction;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Features.CameraSystem
{
    public class CameraHolderService
    {
        //temp solution
        private readonly InteractionCameraHolder _interactionCameraHolder;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public CameraHolderService(Camera mainCamera, InteractionCameraHolder interactionCameraHolder)
        {
            _interactionCameraHolder = interactionCameraHolder;
            MainCamera               = mainCamera;

            if (MainCamera == null) Debug.LogWarning("CameraHolderService was initialized with no main camera");


            _interactionCameraHolder.SetInteractionCamera(MainCamera);
        }

        public Camera MainCamera { get; private set; }

        public void ChangeMainCamera(Camera camera)
        {
            _interactionCameraHolder.SetInteractionCamera(MainCamera);
            MainCamera = camera;
        }
    }
}
