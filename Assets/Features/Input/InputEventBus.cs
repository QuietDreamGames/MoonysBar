using System;
using Features.Input.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Features.Input
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class InputEventBus : IInputEventBusFeed, IInputEventBusSink
    {
        public event Action          OnPointerClick;
        public event Action          OnPointerHoldStart;
        public event Action          OnPointerHoldEnd;
        public event Action<Vector2> OnPointerDrift;
        public event Action<Vector2> OnPointerDrag;
        public event Action          OnPointerDragEnd;

        public void PointerClickFire()
        {
            OnPointerClick?.Invoke();
        }

        public void PointerHoldStartFire()
        {
            OnPointerHoldStart?.Invoke();
        }

        public void PointerHoldEndFire()
        {
            OnPointerHoldEnd?.Invoke();
        }

        public void PointerDriftFire(Vector2 delta)
        {
            OnPointerDrift?.Invoke(delta);
        }

        public void PointerDragFire(Vector2 delta)
        {
            OnPointerDrag?.Invoke(delta);
        }

        public void PointerDragEndFire()
        {
            OnPointerDragEnd?.Invoke();
        }
    }
}
