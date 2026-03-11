using System;
using System.Collections.Generic;
using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.GameStateMachine.States;
using Features.GameSystem.Interfaces.Handlers;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Features.GameStateMachine
{
    public class GameplayStateMachine : BaseStateMachine, IStartable
    {
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public GameplayStateMachine(
            IReadOnlyList<IStartableSystemHandler> startableSystemHandlers,
            IReadOnlyList<IPausableSystemHandler>  pausableSystemHandlers,
            IReadOnlyList<IEndableSystemHandler>   endableSystemHandlers
        ) : base(new Dictionary<Type, IState>())
        {
            States.Add(key: typeof(InitState), value: new InitState(
                stateMachine: this,
                startableSystemHandlers: startableSystemHandlers
            ));

            States.Add(key: typeof(GameloopState), value: new GameloopState(
                pausableSystemHandlers
            ));

            States.Add(key: typeof(PauseState), value: new PauseState(
                pausableSystemHandlers
            ));

            States.Add(key: typeof(EndState), value: new EndState(
                endableSystemHandlers
            ));
        }

        public void Start()
        {
            Enter<InitState>();
        }
    }
}
