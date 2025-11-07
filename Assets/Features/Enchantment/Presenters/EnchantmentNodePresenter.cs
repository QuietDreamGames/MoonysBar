using System;

namespace Features.Enchantment.Presenters
{
    public class EnchantmentNodePresenter : EnchantmentElementPresenterBase
    {
        public void Initialize(object nodeViewModel)
        {
            base.Initialize();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        public event Action OnReturnToPool;

        public void ReturnToPool()
        {
            OnReturnToPool?.Invoke();
            OnReturnToPool = null;

            ClearAnimations();
        }
    }
}
