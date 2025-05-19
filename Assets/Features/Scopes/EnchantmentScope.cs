using Features.Enchantment;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class EnchantmentScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<EnchantmentPlayingFieldService>(Lifetime.Scoped);
        }
    }
}