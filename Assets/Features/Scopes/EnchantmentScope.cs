using Features.Enchantment.Implementations;
using Features.Enchantment.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class EnchantmentScope : LifetimeScope
    {
        [SerializeField] private LineForeshadowElementsFabric lineForeshadowElementsFabric;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(lineForeshadowElementsFabric).As<ILineForeshadowElementsFabric>();

            builder.Register<IEnchantmentPlayingFieldService, EnchantmentPlayingFieldService>(Lifetime.Scoped);
            builder.Register<IEnchantmentForeshadowLineBuilderService,
                EnchantmentForeshadowLineBuilderService>(Lifetime.Scoped);
        }
    }
}
