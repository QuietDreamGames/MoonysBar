using Features.Parameters;
using Features.SceneLoader;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private DefaultRootParametersHolder defaultRootParametersHolder;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoaderService>(Lifetime.Singleton);
            builder.RegisterComponent(defaultRootParametersHolder);
        }
    }
}
