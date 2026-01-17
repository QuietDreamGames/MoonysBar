using System;
using Features.Enchantment.Enums;
using Features.Enchantment.Models;
using Features.Enchantment.Presenters;

namespace Features.Enchantment.Views
{
    public class EnchantmentNodeView : IDisposable
    {
        private EnchantmentNodeViewState _state;

        public EnchantmentNodeView(EnchantmentNodeModel model, EnchantmentNodePresenter presenter)
        {
            Model     = model;
            Presenter = presenter;
            _state    = EnchantmentNodeViewState.UnconnectedIdle;
        }

        private EnchantmentNodeModel     Model     { get; set; }
        private EnchantmentNodePresenter Presenter { get; set; }

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
