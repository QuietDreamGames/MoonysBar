using UnityEngine;

namespace Features.Enchantment.Presenters
{
    public class EnchantmentHandlePresenter : EnchantmentElementPresenterBase
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void HideHandle()
        {
            spriteRenderer.enabled = false;
        }

        public void ShowHandle()
        {
            spriteRenderer.enabled = true;
        }
    }
}
