using Features.MixMinigame.SequenceElements;
using UnityEngine;

namespace Features.MixMinigame
{
    public abstract class MixGameObject : MonoBehaviour
    {
        protected float Timer;
        protected MixGameSequenceElement MixGameSequenceElement;
        
        public void SetSequenceElement(MixGameSequenceElement mixGameSequenceElement)
        {
            MixGameSequenceElement = mixGameSequenceElement;
        }
        
        public void Appear()
        {
            gameObject.SetActive(true);
            Timer = 0;
        }
        
        public abstract void AnimateFailure();
    }
}