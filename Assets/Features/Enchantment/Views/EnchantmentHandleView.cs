using System;
using Features.Enchantment.Presenters;

namespace Features.Enchantment.Views
{
    public class EnchantmentHandleView : IDisposable
    {
        private bool _isHeld;
        private bool _isHovered;

        public EnchantmentHandleView(EnchantmentHandlePresenter presenter)
        {
            Presenter = presenter;
        }

        private EnchantmentHandlePresenter Presenter { get; }

        public void Dispose()
        {
            OnHold   = null;
            OnUnhold = null;

            OnHoverEnter = null;
            OnHoverExit  = null;
        }

        public void HandleHover(bool isHovered)
        {
            switch (isHovered)
            {
                case true when !_isHovered:
                    _isHovered = true;
                    OnHoverEnter?.Invoke();
                    break;
                case false when _isHovered:
                    _isHovered = false;
                    OnHoverExit?.Invoke();
                    break;
            }
        }

        public void HandleHold(bool isHeld)
        {
            switch (isHeld)
            {
                case true when !_isHeld:
                    _isHeld = true;
                    OnHold?.Invoke();
                    break;
                case false when _isHeld:
                    _isHeld = false;
                    OnUnhold?.Invoke();
                    break;
            }
        }

        public event Action OnHold;
        public event Action OnUnhold;

        public event Action OnHoverEnter;
        public event Action OnHoverExit;

        public void Show()
        {
            // Show handle visual
            Presenter.ShowHandle();
        }

        public void Hide()
        {
            // Hide handle visual
            Presenter.HideHandle();
        }

        public bool IsHeld()
        {
            return _isHeld;
        }
    }
}
