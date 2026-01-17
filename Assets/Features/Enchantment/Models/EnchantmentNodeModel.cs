using Features.Enchantment.Datas;

namespace Features.Enchantment.Models
{
    public class EnchantmentNodeModel
    {
        public EnchantmentNodeModel(EnchantmentNodeData data)
        {
            Data = data;
        }

        public EnchantmentNodeData Data { get; }
    }
}
