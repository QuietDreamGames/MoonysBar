using System;
using Features.Collision;
using Features.Interaction.Enums;
using UnityEngine;

namespace Features.Interaction
{
    public readonly struct InteractionEvent
    {
        public InteractionEvent(
            InteractionKind                 kind,
            InteractionPhase                phase,
            ReadOnlyMemory<PointerCollider> targets,
            Vector2                         delta = default
        )
        {
            Kind          = kind;
            Phase         = phase;
            Targets       = targets;
            PrimaryTarget = null;
            Delta         = delta;
            IsMultiple    = true;
        }

        public InteractionEvent(
            InteractionKind  kind,
            InteractionPhase phase,
            PointerCollider  target,
            Vector2          delta = default
        )
        {
            Kind          = kind;
            Phase         = phase;
            Targets       = null;
            PrimaryTarget = target;
            Delta         = delta;
            IsMultiple    = false;
        }

        public InteractionKind  Kind  { get; }
        public InteractionPhase Phase { get; }
        public Vector2          Delta { get; }

        public readonly ReadOnlyMemory<PointerCollider> Targets;
        public readonly PointerCollider                 PrimaryTarget;

        public readonly bool IsMultiple;
    }
}
