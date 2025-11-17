using Features.InputDispatching;
using Features.InputDispatching.Interfaces;
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
        [SerializeField] private InputDispatcher             inputDispatcher;

        protected override void Configure(IContainerBuilder builder)
        {
            // default scene loader:
            builder.Register<SceneLoaderService>(Lifetime.Singleton);

            // input:
            builder.Register<InputEventBus>(Lifetime.Singleton)
                .As<IInputEventBusFeed>()
                .As<IInputEventBusSink>();
            builder.RegisterComponent(inputDispatcher)
                .As<IInputDispatcher>()
                .As<IInputSchemeSelector>();

            // default parameters holder:
            builder.RegisterComponent(defaultRootParametersHolder);
        }
    }
}
