using UnityEngine;

namespace Features.Enchantment.Interfaces
{
    public interface IEnchantmentPlayingFieldService
    {
        public Vector2 GetFieldSize();

        public void SetScreenResolution(int width, int height);

        public Vector2 ConvertRelativeToWorldPosition(Vector2 screenUnscaledPosition);
    }
}
