using Features.Collision;
using Features.Enchantment.Enums;
using UnityEngine;

namespace Features.Enchantment
{
    public class EnchantmentPointerCollider : PointerCollider
    {
        [SerializeField] private EnchantmentPointerColliderType pointerType;

        public EnchantmentPointerColliderType PointerType => pointerType;
    }
}
