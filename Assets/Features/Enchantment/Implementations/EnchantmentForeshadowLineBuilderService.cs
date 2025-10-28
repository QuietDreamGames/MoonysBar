using System.Collections.Generic;
using Features.Enchantment.Datas;
using Features.Enchantment.Interfaces;
using UnityEngine;

namespace Features.Enchantment.Implementations
{
    public class EnchantmentForeshadowLineBuilderService : IEnchantmentForeshadowLineBuilderService
    {
        private List<SpriteRenderer> _lineParts = new();

        public void BuildForeshadowLine(EnchantmentGraphData layout)
        {
        }
    }
}
