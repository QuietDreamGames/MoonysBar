using System;
using System.Collections.Generic;
using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.InputDispatching.Interfaces;
using Features.InputDispatching.PointerStates;
using Features.TimeSystem.Interfaces;
using Features.TimeSystem.Interfaces.Injected;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Features.InputDispatching
{
    public class InputPointerStateMachine : BaseStateMachine, IStartable
    {
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public InputPointerStateMachine(
            IInputDispatcher        inputDispatcher,
            IInputEventBusSink      inputEventBusSink,
            ITimeSystem             timeSystem,
            IUpdateProvider         updateProvider,
            ITransientTimeCollector transientTimeCollector
        ) : base(new Dictionary<Type, IState>())
        {
            States.Add(key: typeof(IdlePointerState),
                value: new IdlePointerState(stateMachine: this, inputSystem: inputDispatcher));
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

            var pendingPointerState = new PendingPointerState(
                stateMachine: this,
                inputDispatcher: inputDispatcher,
                inputEventBus: inputEventBusSink
            );

            States.Add(key: typeof(PendingPointerState), value: pendingPointerState);
            transientTimeCollector.UpdateHandlers.Add(pendingPointerState);

            // not implemented
            States.Add(key: typeof(MovePointerState), value: new MovePointerState());
            States.Add(key: typeof(HoverPointerState), value: new HoverPointerState(inputDispatcher));

            timeSystem.Subscribe(transientTimeCollector);
            timeSystem.SetUpdateProvider(updateProvider);
            timeSystem.Initialize();
        }

        public void Start()
        {
            Enter<IdlePointerState>();
        }
    }
}
