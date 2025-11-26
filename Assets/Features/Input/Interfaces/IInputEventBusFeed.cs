using System;
using UnityEngine;

namespace Features.Input.Interfaces
{
    public interface IInputEventBusFeed
    {
        event Action          OnPointerClick;
        event Action          OnPointerHoldStart;
        event Action          OnPointerHoldEnd;
        event Action<Vector2> OnPointerDrift;
        event Action<Vector2> OnPointerDrag;
        event Action          OnPointerDragEnd;
    }
}
