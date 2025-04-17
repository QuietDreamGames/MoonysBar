using Features.CameraSystem;
using Features.InputDispatching;
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
            builder.RegisterInstance(mainCamera).As<Camera>().AsSelf();
            
            builder.Register<CameraHolderService>(Lifetime.Scoped);
            
            builder.Register<InputDispatcher>(Lifetime.Scoped);
            builder.Register<InputService>(Lifetime.Scoped);
            builder.Register<InputPointerGameObjectsCollisionService>(Lifetime.Scoped);
        }
    }
}