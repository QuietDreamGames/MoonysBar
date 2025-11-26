using System;
using Features.Collision;
using JetBrains.Annotations;
using UnityEngine;

namespace Features.Interaction
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class InteractionEventBus
    {
        public event Action<PointerCollider>          OnObjectClicked;
        public event Action<PointerCollider>          OnObjectHoldStart;
        public event Action<PointerCollider>          OnObjectHoldEnd;
        public event Action<PointerCollider, Vector2> OnObjectDrag;
        public event Action<PointerCollider>          OnObjectDragEnd;

        public void ObjectClickedFire(PointerCollider pointerCollider)
        {
            OnObjectClicked?.Invoke(pointerCollider);
        }

        public void ObjectHoldStartFire(PointerCollider pointerCollider)
        {
            OnObjectHoldStart?.Invoke(pointerCollider);
        }

        public void ObjectHoldEndFire(PointerCollider pointerCollider)
        {
            OnObjectHoldEnd?.Invoke(pointerCollider);
        }

        public void ObjectDragFire(PointerCollider pointerCollider, Vector2 delta)
        {
            OnObjectDrag?.Invoke(arg1: pointerCollider, arg2: delta);
        }

        public void ObjectDragEndFire(PointerCollider pointerCollider)
        {
            OnObjectDragEnd?.Invoke(pointerCollider);
        }
    }
}
