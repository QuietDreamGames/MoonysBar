using JetBrains.Annotations;
using UnityEngine;

namespace Features.Interaction
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class InteractionCameraHolder
    {
        private Camera _interactionCamera;

        public bool TryGetInteractionCamera(out Camera camera)
        {
            camera = _interactionCamera;
            return camera != null;
        }
    }
}
