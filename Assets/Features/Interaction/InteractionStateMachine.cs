using System;
using System.Collections.Generic;
using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Input.Interfaces;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Features.Interaction
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class InteractionStateMachine : BaseStateMachine, IStartable
    {
        public InteractionStateMachine(
            IInputEventBusFeed inputEventBusFeed
        ) : base(new Dictionary<Type, IState>())
        {
        }

        public void Start()
        {
        }
    }
}
