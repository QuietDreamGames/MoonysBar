namespace Features.Interaction.Interfaces
{
    public interface IInteractionEventBusSink
    {
        // Capability flags to allow the event bus to optimize event grouping/behaviour.
        bool SupportsMultipleHits   { get; }
        bool SupportsPrematureExits { get; }
        bool SupportsCollects       { get; }

        // Single unified handler. Inspect InteractionEvent.Kind, .Phase, .Targets, and .Delta.
        void HandleInteraction(InteractionEvent interactionEvent);
    }
}
