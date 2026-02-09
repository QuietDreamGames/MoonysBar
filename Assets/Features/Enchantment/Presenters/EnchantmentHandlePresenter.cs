using UnityEngine;

namespace Features.Enchantment.Presenters
{
    public class EnchantmentHandlePresenter : EnchantmentElementPresenterBase
    {
        [SerializeField] private SpriteRenderer             spriteRenderer;
        [SerializeField] private EnchantmentPointerCollider pointerCollider;


        private float _pointerColliderDefaultRadius;
        private bool  _wasActivatedLastFrame = false;

        // public override void OnUpdate(float deltaTime)
        // {
        //     base.OnUpdate(deltaTime);
        //
        //     if (!_wasActivatedLastFrame) return;
        //     _wasActivatedLastFrame = false;
        //     if (pointerCollider.Collider != null && pointerCollider.Collider is CircleCollider2D circleCollider)
        //         circleCollider.radius = _pointerColliderDefaultRadius;
        // }
        //
        // private void Update()
        // {
        //     if (!_wasActivatedLastFrame) return;
        //     _wasActivatedLastFrame = false;
        //     if (pointerCollider.Collider != null && pointerCollider.Collider is CircleCollider2D circleCollider)
        //         circleCollider.radius = _pointerColliderDefaultRadius;
        // }

        public void Activate()
        {
            spriteRenderer.enabled           = false;
            pointerCollider.Collider.enabled = false;

            // kind of a hack, but there is a timing issue where if we don't delay this by a frame,
            // the pointer might not be collected
            _wasActivatedLastFrame = true;
            if (pointerCollider.Collider == null ||
                pointerCollider.Collider is not CircleCollider2D circleCollider) return;
            _pointerColliderDefaultRadius = circleCollider.radius;
            // circleCollider.radius         = _pointerColliderDefaultRadius * 30;
        }

        public void Deactivate()
        {
            spriteRenderer.enabled           = true;
            pointerCollider.Collider.enabled = true;
        }
    }
}
