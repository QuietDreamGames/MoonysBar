using Features.Collision;
using UnityEngine;

namespace Features.MixMinigame
{
    public class MixGamePointerCollider : PointerCollider
    {
        [SerializeField] private bool isClickable;

        public bool IsClickable => isClickable;
    }
}
