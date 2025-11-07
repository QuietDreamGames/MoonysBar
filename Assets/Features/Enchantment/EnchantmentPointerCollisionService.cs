using Features.Collision;
using Features.Enchantment.Presenters;
using Features.InputDispatching;
using JetBrains.Annotations;
using VContainer;

namespace Features.Enchantment
{
    public class EnchantmentPointerCollisionService
    {
        private readonly EnchantmentElementsHolderAndUpdater _enchantmentElementsHolderAndUpdater;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public EnchantmentPointerCollisionService(
            InputPointerCollisionService        inputPointerCollisionService,
            EnchantmentElementsHolderAndUpdater enchantmentElementsHolderAndUpdater
        )
        {
            inputPointerCollisionService.OnHeldPointerColliderAction    += OnPointerColliderHeld;
            inputPointerCollisionService.OnHoveredPointerColliderAction += OnPointerColliderHovered;

            _enchantmentElementsHolderAndUpdater = enchantmentElementsHolderAndUpdater;
        }

        private void OnPointerColliderHeld(PointerCollider pointerCollider, bool isHeld)
        {
            if (pointerCollider is not EnchantmentPointerCollider) return;
            var elementView = pointerCollider.GetComponentInParent<EnchantmentElementPresenterBase>();
            if (!elementView) return;

            if (elementView is EnchantmentNodePresenter nodePresenter)
            {
                if (_enchantmentElementsHolderAndUpdater.TryFindEnchantmentNodeByPresenter(
                        presenter: nodePresenter,
                        result: out var node
                    ))
                    node.Item3.HandleHold(isHeld);
            }
            else if (elementView is EnchantmentHandlePresenter)
            {
                if (_enchantmentElementsHolderAndUpdater.TryGetEnchantmentHandle(out var handle))
                    handle.Item2.HandleHold(isHeld);
            }
        }

        private void OnPointerColliderHovered(PointerCollider pointerCollider, bool isHovered)
        {
            if (pointerCollider is not EnchantmentPointerCollider) return;
            var elementView = pointerCollider.GetComponentInParent<EnchantmentElementPresenterBase>();
            if (!elementView) return;

            if (elementView is EnchantmentNodePresenter nodePresenter)
            {
                if (_enchantmentElementsHolderAndUpdater.TryFindEnchantmentNodeByPresenter(
                        presenter: nodePresenter,
                        result: out var node
                    ))
                    node.Item3.HandleHover(isHovered);
            }
            else if (elementView is EnchantmentHandlePresenter handlePresenter)
            {
                if (_enchantmentElementsHolderAndUpdater.TryGetEnchantmentHandle(out var handle))
                    handle.Item2.HandleHover(isHovered);
            }
        }
    }
}
