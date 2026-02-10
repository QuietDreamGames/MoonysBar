using System;
using System.Collections.Generic;
using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.GameStateMachine.States;
using Features.TimeSystem.Interfaces;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Features.GameStateMachine
{
    public class GameplayStateMachine : BaseStateMachine, IStartable
    {
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public GameplayStateMachine(
            ITimeSystem gameplayTimeSystem
        ) : base(new Dictionary<Type, IState>())
        {
            States.Add(key: typeof(InitState), value: new InitState(
                stateMachine: this,
                gameplayTimeSystem
            ));

            States.Add(key: typeof(GameloopState), value: new GameloopState(
                gameplayTimeSystem
            ));

            States.Add(key: typeof(PauseState), value: new PauseState(
                gameplayTimeSystem
            ));
        }

        public void Start()
        {
            Enter<InitState>();
        }
    }
}
