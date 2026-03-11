#nullable enable

using System;
using System.Collections.Generic;
using Features.HierarchicalStateMachine.Interfaces;

namespace Features.HierarchicalStateMachine
{
    public abstract class BaseHierarchicalState : IState, IMachine
    {
        protected readonly Dictionary<IEvent, IState> Transitions;
        protected readonly Dictionary<Type, IState>   States;
        private readonly   IState?                    _currentState = null;

        protected BaseHierarchicalState(Dictionary<IEvent, IState> transitions, Dictionary<Type, IState> states)
        {
            Transitions = transitions;
            States      = states;
        }


        public void Enter()
        {
            _currentState?.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public void Exit()
        {
            _currentState?.Exit();
        }
    }
}
