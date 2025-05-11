using Features.CameraSystem;
using Features.GameStateMachine;
using Features.InputDispatching;
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
        [SerializeField] private Camera          mainCamera;
        [SerializeField] private InputDispatcher inputDispatcher;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(mainCamera);

            builder.Register<CameraHolderService>(Lifetime.Singleton);

            builder.RegisterEntryPoint<InjectedTimeUpdateProvider>(Lifetime.Singleton).As<IUpdateProvider>();
            builder.Register<ITimeSystem, InjectedTimeSystem>(Lifetime.Singleton);
            builder.Register<ITransientTimeCollector, InjectedTimeCollector>(Lifetime.Transient);

            builder.Register<GameplayStateMachine>(Lifetime.Singleton);

            builder.RegisterComponent(inputDispatcher);
            builder.Register<InputService>(Lifetime.Singleton);
            builder.Register<InputPointerGameObjectsCollisionService>(Lifetime.Singleton);
        }
    }
}
