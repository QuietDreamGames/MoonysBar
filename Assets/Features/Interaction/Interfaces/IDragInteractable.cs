using UnityEngine;

namespace Features.Interaction.Interfaces
{
    public interface IDragInteractable
    {
        void OnDrag(Vector2 delta);
        void OnDragEnd();
    }
}
