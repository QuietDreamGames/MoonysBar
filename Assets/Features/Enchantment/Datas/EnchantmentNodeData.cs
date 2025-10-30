using UnityEngine;

namespace Features.Enchantment.Datas
{
    public class EnchantmentNodeData
    {
        public EnchantmentNodeData(
            int     index,
            Vector2 position)
        {
            Index    = index;
            Position = position;
        }

        public int Index { get; }

        public Vector2 Position { get; }
    }
}
