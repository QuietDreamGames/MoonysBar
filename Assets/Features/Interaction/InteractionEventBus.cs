using System;
using System.Collections.Generic;
using Features.Interaction.Enums;
using Features.Interaction.Helpers;
using Features.Interaction.Interfaces;
using JetBrains.Annotations;

namespace Features.Interaction
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class InteractionEventBus : IInteractionEventBusFeed, IInteractionEventBusSink
    {
        private readonly Dictionary<InteractionPhase, List<Subscriber>> _subsByPhase
            = new();

        public bool SupportsMultipleHits   => CheckMultipleHitsSubscribers();
        public bool SupportsPrematureExits => CheckPrematureExitSubscribers();
        public bool SupportsCollects       => CheckCollectSubscribers();

        private bool CheckMultipleHitsSubscribers()
        {
            foreach (var subs in _subsByPhase.Values)
            foreach (var sub in subs)
                if (sub.SupportsMultipleHits)
                    return true;

            return false;
        }

        private bool CheckPrematureExitSubscribers()
        {
            return _subsByPhase.TryGetValue(key: InteractionPhase.PrematureExit, value: out var list) && list.Count > 0;
        }

        private bool CheckCollectSubscribers()
        {
            return _subsByPhase.TryGetValue(key: InteractionPhase.Collect, value: out var list) && list.Count > 0;
        }

        public void HandleInteraction(InteractionEvent interactionEvent)
        {
            if (!_subsByPhase.TryGetValue(key: interactionEvent.Phase, value: out var bucket))
                return;

            foreach (var s in bucket)
                if ((s.Kinds & interactionEvent.Kind) != 0)
                    s.Handler(interactionEvent);
        }

        public IDisposable Subscribe(
            InteractionKind          kinds,
            InteractionPhase         phases,
            bool                     supportsMultipleHits,
            Action<InteractionEvent> handler
        )
        {
            var subscriber = new Subscriber
            {
                Kinds                = kinds,
                Phases               = phases,
                SupportsMultipleHits = supportsMultipleHits,
                Handler              = handler
            };

            foreach (InteractionPhase phase in Enum.GetValues(typeof(InteractionPhase)))
                if (phases.HasFlag(phase))
                {
                    if (!_subsByPhase.TryGetValue(key: phase, value: out var subs))
                    {
                        subs                = new List<Subscriber>();
                        _subsByPhase[phase] = subs;
                    }

                    subs.Add(subscriber);
                }

            return new InteractionSubscriptionDisposable(
                onDispose: () =>
                {
                    foreach (InteractionPhase phase in Enum.GetValues(typeof(InteractionPhase)))
                        if (phases.HasFlag(phase) &&
                            _subsByPhase.TryGetValue(key: phase, value: out var subs))
                        {
                            subs.Remove(subscriber);
                            if (subs.Count == 0)
                                _subsByPhase.Remove(phase);
                        }
                }
            );
        }

        private sealed class Subscriber
        {
            public InteractionKind          Kinds;
            public InteractionPhase         Phases;
            public bool                     SupportsMultipleHits;
            public Action<InteractionEvent> Handler;
        }
    }
}
