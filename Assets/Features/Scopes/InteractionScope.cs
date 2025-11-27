using Features.Interaction;
using Features.Interaction.Helpers;
using Features.TimeSystem.Core.Injected;
using Features.TimeSystem.Interfaces;
using Features.TimeSystem.Interfaces.Injected;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class InteractionScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<InteractionHitRegistrator>(Lifetime.Singleton);
            builder.Register<InteractionRayCollisionBuffer>(Lifetime.Transient);
            builder.Register<InteractionPointerCollisionBuffer>(Lifetime.Transient);

            builder.RegisterEntryPoint<InteractionStateMachine>(Lifetime.Singleton);

            builder.Register<ITimeSystem, InjectedTimeSystem>(Lifetime.Singleton);
            builder.RegisterEntryPoint<InjectedTimeUpdateProvider>(Lifetime.Singleton).As<IUpdateProvider>();
            builder.Register<ITransientTimeCollector, InjectedTimeCollector>(Lifetime.Transient);
        }
    }
}
