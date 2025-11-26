using System;
using System.Collections.Generic;
using Features.Collision;
using Features.Input;
using Features.Interaction.Helpers;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Features.Interaction
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class InteractionHitRegistrator
    {
        [Inject] private InteractionCameraHolder       _interactionCameraHolder;
        [Inject] private InteractionRayCollisionBuffer _rayCollisionBuffer;

        public bool TryHitPointerCollider(out PointerCollider collider)
        {
            if (!_interactionCameraHolder.TryGetInteractionCamera(out var camera) ||
                !InputUtils.TryGetPrimaryPointerScreenPosition(out var pointerScreenPosition))
            {
                collider = null;
                return false;
            }

            var ray = camera.ScreenPointToRay(pointerScreenPosition);
            if (Physics.Raycast(ray: ray, hitInfo: out var hitInfo)
                && hitInfo.collider.TryGetComponent(out collider)) return true;

            collider = null;
            return false;
        }

        public bool TryHitAllPointerColliders(out PointerCollider[] colliders)
        {
            return TryHitAllPointerCollidersWithDelta(delta: Vector2.zero, colliders: out colliders);
        }

        public bool TryHitAllPointerCollidersWithDelta(Vector2 delta, out PointerCollider[] colliders)
        {
            if (!_interactionCameraHolder.TryGetInteractionCamera(out var camera) ||
                !InputUtils.TryGetPrimaryPointerScreenPosition(out var pointerScreenPosition))
            {
                colliders = Array.Empty<PointerCollider>();
                return false;
            }

            var ray      = camera.ScreenPointToRay(pointerScreenPosition + delta);
            var hitCount = Physics.RaycastNonAlloc(ray: ray, results: _rayCollisionBuffer.HitsBuffer);
            if (hitCount == 0)
            {
                colliders = Array.Empty<PointerCollider>();
                return false;
            }

            var list = new List<PointerCollider>(hitCount);
            for (var i = 0; i < hitCount; i++)
            {
                var hit = _rayCollisionBuffer.HitsBuffer[i];
                if (hit.collider != null && hit.collider.TryGetComponent(out PointerCollider pointerCollider))
                    list.Add(pointerCollider);
            }

            if (list.Count == 0)
            {
                colliders = Array.Empty<PointerCollider>();
                return false;
            }

            colliders = list.ToArray();
            return true;
        }

        // public bool TryHitAllPointerCollidersWithLine(Vector2 start, Vector2 end, out PointerCollider[] colliders)
        // {
        //     if (!_interactionCameraHolder.TryGetInteractionCamera(out var camera))
        //     {
        //         colliders = Array.Empty<PointerCollider>();
        //         return false;
        //     }
        //
        //     var lineStart = camera.ScreenToWorldPoint(start);
        //     var lineEnd   = camera.ScreenToWorldPoint(end);
        //
        //     var hitsDetected = Physics.Linecast(start: lineStart, end: lineEnd, out var hitInfo);
        //
        //     if (hitsDetected && hitInfo.collider.TryGetComponent(out PointerCollider pointerCollider))
        //     {
        //         colliders = new[] { pointerCollider };
        //         return true;
        //     }
        //     else
        //     {
        //         colliders = Array.Empty<PointerCollider>();
        //         return false;
        //     }
        // }
    }
}
