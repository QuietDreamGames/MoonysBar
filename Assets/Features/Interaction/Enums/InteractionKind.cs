using System;

namespace Features.Interaction.Enums
{
    [Flags]
    public enum InteractionKind
    {
        Click = 1 << 0,
        Hold  = 1 << 1,
        Drag  = 1 << 2
    }
}
