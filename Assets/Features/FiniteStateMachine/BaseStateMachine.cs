#nullable enable

using System;
using System.Collections.Generic;
using Features.FiniteStateMachine.Interfaces;

namespace Features.FiniteStateMachine
{
    public abstract class BaseStateMachine : IMachine
    {
        private IState? _currentState = null;

        protected readonly Dictionary<Type, IState> States;

        protected BaseStateMachine(Dictionary<Type, IState> states)
        {
            States = states;
        }

        public void Enter<TState>()
        {
            _currentState?.Exit();
            if (States.TryGetValue(typeof(TState), out _currentState))
            {
                _currentState.Enter();
            }
            else
            {
                throw new ArgumentException($"State {typeof(TState).FullName} is not found.");
            }
        }
    }
}