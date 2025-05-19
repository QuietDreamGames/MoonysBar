using UnityEngine;

namespace Features.Enchantment.Datas
{
    public class EnchantmentNodeData
    {
        public EnchantmentNodeData(
            int     index,
            Vector2 initialPosition)
        {
            Index           = index;
            InitialPosition = initialPosition;
        }

        public int Index { get; }

        public Vector2 InitialPosition { get; }
    }
}
