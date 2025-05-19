using System;
using System.Collections.Generic;
using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.GameStateMachine.States;
using Features.TimeSystem.Interfaces;
using JetBrains.Annotations;

namespace Features.GameStateMachine
{
    public class GameplayStateMachine : BaseStateMachine
    {
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public GameplayStateMachine(
            ITimeSystem gameplayTimeSystem
        ) : base(new Dictionary<Type, IState>())
        {
            States.Add(typeof(InitState), new InitState(
                this,
                gameplayTimeSystem
            ));

            States.Add(typeof(GameloopState), new GameloopState(
                gameplayTimeSystem
            ));

            States.Add(typeof(EndLoseState), new EndLoseState());

            States.Add(typeof(EndWinState), new EndWinState());
        }
    }
}