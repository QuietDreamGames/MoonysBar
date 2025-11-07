using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.InputDispatching
{
    public static class InputUtils
    {
        public static Vector2 GetPrimaryPointerScreenPosition()
        {
            return Mouse.current != null
                ? Mouse.current.position.ReadValue()
                // : Touchscreen.current?.primaryTouch.position.ReadValue() ?? Vector2.zero;
                : Touchscreen.current?.primaryTouch.position.ReadValue() ??
                  throw new Exception("No mouse or touchscreen found");
        }
    }
}
