using JetBrains.Annotations;
using UnityEngine;

namespace Features.Interaction.Helpers
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class InteractionRayCollisionBuffer
    {
        public readonly RaycastHit[] HitsBuffer = new RaycastHit[10];
    }
}
