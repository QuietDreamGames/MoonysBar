using Features.Enchantment.Implementations;
using Features.Enchantment.Interfaces;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class EnchantmentScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IEnchantmentPlayingFieldService, EnchantmentPlayingFieldService>(Lifetime.Scoped);
        }
    }
}
