using Features.TimeSystem.Interfaces.Handlers;
using UnityEngine;

namespace Features.MixMinigame.Views
{
    public abstract class MixGameTileView : MonoBehaviour, IUpdateHandler
    {
        public void Appear()
        {
            gameObject.SetActive(true);
        }
        
        public abstract void AnimateFailure();
        
        public virtual void OnUpdate(float deltaTime)
        {
            
        }
    }
}