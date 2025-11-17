using UnityEngine;

namespace Features.InputDispatching.Interfaces
{
    public interface IInputEventBusSink
    {
        void PointerClickFire();
        void PointerHoldStartFire();
        void PointerHoldEndFire();
        void PointerDriftFire(Vector2 delta);
        void PointerDragFire(Vector2  delta);
        void PointerDragEndFire();
    }
}
