using System;
using Features.Enchantment.Enums;
using Features.Enchantment.Models;

namespace Features.Enchantment.Views
{
    public class EnchantmentNodeView : IDisposable
    {
        private EnchantmentNodeViewState _state;

        public EnchantmentNodeView(EnchantmentNodeModel nodeModel)
        {
            NodeModel = nodeModel;
            _state    = EnchantmentNodeViewState.UnconnectedIdle;
        }

        private EnchantmentNodeModel NodeModel { get; set; }

        public void Dispose()
        {
        }

        public void SetState(EnchantmentNodeViewState newState)
        {
            _state = newState;
        }

        public EnchantmentNodeViewState GetState()
        {
            return _state;
        }
    }
}
