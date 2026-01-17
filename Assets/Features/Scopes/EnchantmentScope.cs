using Features.Enchantment;
using Features.Enchantment.Factories;
using Features.Enchantment.Implementations;
using Features.Enchantment.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class EnchantmentScope : LifetimeScope
    {
        [SerializeField] private LineForeshadowElementsFactory lineForeshadowElementsFactory;
        [SerializeField] private EnchantmentElementsFactory    enchantmentElementsFactory;


        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(lineForeshadowElementsFactory).As<ILineForeshadowElementsFactory>();
            builder.RegisterInstance(enchantmentElementsFactory).As<IEnchantmentElementsFactory>();

            builder.Register<EnchantmentElementsHolderAndUpdater>(Lifetime.Scoped);

            builder.Register<IEnchantmentPlayingFieldService, EnchantmentPlayingFieldService>(Lifetime.Scoped);
            builder.Register<IEnchantmentForeshadowLineBuilderService,
                EnchantmentForeshadowLineBuilderService>(Lifetime.Scoped);
            builder.Register<EnchantmentPathController>(Lifetime.Scoped);
        }
    }
}
