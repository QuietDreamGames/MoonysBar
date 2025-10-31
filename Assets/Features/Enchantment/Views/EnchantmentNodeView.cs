using System;
using Features.View;

namespace Features.Enchantment.Views
{
    public class EnchantmentNodeView : TweenedView
    {
        public  void Initialize(object nodeViewModel)
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
