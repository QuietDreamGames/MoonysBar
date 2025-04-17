using Features.SceneLoader;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoaderService>(Lifetime.Singleton);
        }
    }
}