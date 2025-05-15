using UnityEngine;

namespace Features.Collision
{
    [RequireComponent(typeof(Collider2D))]
    public class PointerCollider : MonoBehaviour
    {
        [SerializeField] private Collider2D pointerCollider;

        public Collider2D Collider => pointerCollider;

        private void OnValidate()
        {
            if (!pointerCollider) pointerCollider = GetComponent<Collider2D>();
        }
    }
}
