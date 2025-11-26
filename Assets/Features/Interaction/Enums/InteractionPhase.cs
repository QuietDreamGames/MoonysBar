namespace Features.Interaction.Enums
{
    public enum InteractionPhase
    {
        Action        = 0, // single action like a click
        Start         = 1, // hold/drag start
        Collect       = 2, // intermediate collection (for multi-target hold/drag)
        Move          = 3, // incremental drag movement
        End           = 4, // normal end
        PrematureExit = 5  // exited prematurely
    }
}
