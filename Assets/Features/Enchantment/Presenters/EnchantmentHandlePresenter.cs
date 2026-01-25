using UnityEngine;

namespace Features.Enchantment.Presenters
{
    public class EnchantmentHandlePresenter : EnchantmentElementPresenterBase
    {
        [SerializeField] private SpriteRenderer             spriteRenderer;
        [SerializeField] private EnchantmentPointerCollider pointerCollider;

        public void HideHandle()
        {
            spriteRenderer.enabled           = false;
            pointerCollider.Collider.enabled = false;
        }

        public void ShowHandle()
        {
            spriteRenderer.enabled           = true;
            pointerCollider.Collider.enabled = true;
        }
    }
}
