using System;
using System.Collections.Generic;
using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Input.Interfaces;
using Features.Input.PointerStates;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Features.Input
{
    public class InputPointerStateMachine : BaseStateMachine, IStartable
    {
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public InputPointerStateMachine(
            IInputDispatcher        inputDispatcher,
            IInputEventBusSink      inputEventBusSink
        ) : base(new Dictionary<Type, IState>())
        {
            States.Add(key: typeof(IdlePointerState),
                value: new IdlePointerState(
                    stateMachine: this,
                    inputDispatcher: inputDispatcher,
                    inputEventBus: inputEventBusSink
                ));
            States.Add(key: typeof(HoldPointerState), value: new HoldPointerState(
                stateMachine: this,
                inputDispatcher: inputDispatcher,
                inputEventBus: inputEventBusSink
            ));
            States.Add(key: typeof(DragPointerState), value: new DragPointerState(
                stateMachine: this,
                inputDispatcher: inputDispatcher,
                inputEventBus: inputEventBusSink
            ));

            // not implemented
            States.Add(key: typeof(HoverPointerState), value: new HoverPointerState(inputDispatcher));
        }

        public void Start()
        {
            Enter<IdlePointerState>();
        }
    }
}
