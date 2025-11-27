using JetBrains.Annotations;
using UnityEngine;

namespace Features.Interaction.Helpers
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class InteractionRayCollisionBuffer
    {
        public readonly RaycastHit2D[] HitsBuffer = new RaycastHit2D[10];
    }
}
