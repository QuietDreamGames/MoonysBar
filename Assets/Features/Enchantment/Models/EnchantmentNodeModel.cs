using Features.Enchantment.Datas;

namespace Features.Enchantment.Models
{
    public class EnchantmentNodeModel
    {
        public EnchantmentNodeModel(EnchantmentNodeData data, int maxConnections)
        {
            Data           = data;
            MaxConnections = maxConnections;
        }

        public EnchantmentNodeData Data { get; }

        public int MaxConnections { get; }
    }
}
