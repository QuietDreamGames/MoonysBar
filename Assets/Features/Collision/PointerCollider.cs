using UnityEngine;

namespace Features.Collision
{
    public class PointerCollider : MonoBehaviour
    {
        [SerializeField] private Collider2D pointerCollider;

        public Collider2D Collider => pointerCollider;
    }
}
