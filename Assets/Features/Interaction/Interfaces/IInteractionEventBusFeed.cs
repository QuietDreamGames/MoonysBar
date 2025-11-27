using System;
using Features.Interaction.Enums;

namespace Features.Interaction.Interfaces
{
    public interface IInteractionEventBusFeed
    {
        IDisposable Subscribe(
            InteractionKind          kinds,
            InteractionPhase         phases,
            bool                     supportsMultipleHits,
            Action<InteractionEvent> handler
        );
    }
}
