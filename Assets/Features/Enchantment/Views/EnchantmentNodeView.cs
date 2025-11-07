using System;
using Features.Enchantment.Enums;
using Features.Enchantment.Models;

namespace Features.Enchantment.Views
{
    public class EnchantmentNodeView : IDisposable
    {
        public EnchantmentNodeView(EnchantmentNodeModel nodeModel)
        {
            NodeModel = nodeModel;
            State     = EnchantmentNodeViewState.UnconnectedIdle;
        }

        public EnchantmentNodeViewState State { get; private set; }

        private EnchantmentNodeModel NodeModel { get; set; }

        public void Dispose()
        {
        }

        public void HandleHover(bool isHovered)
        {
        }

        public void HandleHold(bool isHeld)
        {
        }
    }
}
