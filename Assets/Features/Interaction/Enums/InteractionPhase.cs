using System;

namespace Features.Interaction.Enums
{
    [Flags]
    public enum InteractionPhase
    {
        Action        = 1 << 0, // single action like a click
        Start         = 1 << 1, // hold/drag start
        Collect       = 1 << 2, // intermediate collection (for multi-target hold/drag)
        Move          = 1 << 3, // incremental drag movement
        End           = 1 << 4, // normal end
        PrematureExit = 1 << 5  // exited prematurely
    }
}
