using Features.CameraSystem;
using Features.GameStateMachine;
using Features.TimeSystem.Core.Injected;
using Features.TimeSystem.Interfaces;
using Features.TimeSystem.Interfaces.Injected;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private Camera mainCamera;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(mainCamera);

            builder.RegisterEntryPoint<CameraHolderService>();

            builder.RegisterEntryPoint<InjectedTimeUpdateProvider>().As<IUpdateProvider>();
            builder.Register<ITimeSystem, InjectedTimeSystem>(Lifetime.Singleton);
            builder.Register<ITransientTimeCollector, InjectedTimeCollector>(Lifetime.Transient);

            builder.Register<GameplayStateMachine>(Lifetime.Singleton);
        }
    }
}
