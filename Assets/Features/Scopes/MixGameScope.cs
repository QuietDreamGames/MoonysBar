using Features.MixMinigame;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class MixGameScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MixGameEntryPoint>();
        }
        
    }
}