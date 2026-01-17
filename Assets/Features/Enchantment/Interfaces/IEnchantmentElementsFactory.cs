using Features.Enchantment.Datas;
using Features.Enchantment.Models;
using Features.Enchantment.Presenters;
using Features.Enchantment.Views;

namespace Features.Enchantment.Interfaces
{
    public interface IEnchantmentElementsFactory
    {
        (EnchantmentNodeModel, EnchantmentNodePresenter, EnchantmentNodeView) CreateEnchantmentNode(
            EnchantmentNodeData data);

        (EnchantmentHandlePresenter, EnchantmentHandleView) CreateEnchantmentHandle();
    }
}