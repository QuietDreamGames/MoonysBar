using Features.MixMinigame;
using Features.MixMinigame.Factories;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class MixGameScope : LifetimeScope
    {
        [SerializeField] private MixGameTileFactory mixGameTileFactory;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(mixGameTileFactory);
            builder.Register<MixGamePlayingFieldService>(Lifetime.Scoped);
            builder.RegisterEntryPoint<MixGameEntryPoint>();
        }
    }
}