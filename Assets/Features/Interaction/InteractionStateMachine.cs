using System;
using System.Collections.Generic;
using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Input.Interfaces;
using Features.Interaction.Helpers;
using Features.Interaction.InteractionStates;
using Features.Interaction.Interfaces;
using Features.TimeSystem.Interfaces;
using Features.TimeSystem.Interfaces.Handlers;
using Features.TimeSystem.Interfaces.Injected;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Features.Interaction
{
    public class InteractionStateMachine : BaseStateMachine, IStartable
    {
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public InteractionStateMachine(
            IInputEventBusFeed                inputEventBusFeed,
            IInteractionEventBusSink          interactionEventBusSink,
            InteractionHitRegistrator         hitRegistrator,
            InteractionPointerCollisionBuffer collisionBuffer,
            ITimeSystem                       timeSystem,
            ITransientTimeCollector           transientTimeCollector
        ) : base(new Dictionary<Type, IState>())
        {
            States.Add(key: typeof(IdleInteractionState),
                value: new IdleInteractionState(
                    stateMachine: this,
                    inputEventBusFeed: inputEventBusFeed,
                    hitRegistrator: hitRegistrator,
                    collisionBuffer: collisionBuffer,
                    interactionEventBusSink: interactionEventBusSink
                ));

            States.Add(key: typeof(HoldInteractionState),
                value: new HoldInteractionState(
                    stateMachine: this,
                    inputEventBusFeed: inputEventBusFeed,
                    hitRegistrator: hitRegistrator,
                    collisionBuffer: collisionBuffer,
                    interactionEventBusSink: interactionEventBusSink
                ));

            States.Add(key: typeof(DragInteractionState),
                value: new DragInteractionState(
                    stateMachine: this,
                    inputEventBusFeed: inputEventBusFeed,
                    hitRegistrator: hitRegistrator,
                    collisionBuffer: collisionBuffer,
                    interactionEventBusSink: interactionEventBusSink
                ));

            // iterate through states and add them to the time system
            foreach (var state in States.Values)
                if (state is IUpdateHandler updatableState)
                    transientTimeCollector.UpdateHandlers.Add(updatableState);

            timeSystem.Subscribe(transientTimeCollector);
            timeSystem.Initialize();
        }

        public void Start()
        {
            Enter<IdleInteractionState>();
        }
    }
}
