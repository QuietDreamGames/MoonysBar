using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Input
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

        public static bool TryGetPrimaryPointerScreenPosition(out Vector2 position)
        {
            if (Mouse.current != null)
            {
                position = Mouse.current.position.ReadValue();
                return true;
            }

            if (Touchscreen.current != null)
            {
                position = Touchscreen.current.primaryTouch.position.ReadValue();
                return true;
            }

            position = default;
            return false;
        }
    }
}
