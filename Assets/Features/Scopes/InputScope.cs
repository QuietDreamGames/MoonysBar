using Features.Input;
using Features.Input.Interfaces;
using Features.TimeSystem.Core.Injected;
using Features.TimeSystem.Interfaces;
using Features.TimeSystem.Interfaces.Injected;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class InputScope : LifetimeScope
    {
        [SerializeField] private InputDispatcher inputDispatcher;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ITimeSystem, InjectedTimeSystem>(Lifetime.Singleton);
            builder.RegisterEntryPoint<InjectedTimeUpdateProvider>(Lifetime.Singleton).As<IUpdateProvider>();
            builder.Register<ITransientTimeCollector, InjectedTimeCollector>(Lifetime.Transient);

            builder.RegisterEntryPoint<InputTimeSystemInitializer>();
            builder.RegisterEntryPoint<InputClickTimer>().As<IInputClickTimer>();
            builder.RegisterEntryPoint<InputPointerStateMachine>();

            builder.RegisterEntryPoint<InputPointerDebugger>(Lifetime.Singleton);
        }
    }
}
