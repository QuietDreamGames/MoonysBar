using UnityEngine;

namespace Features.MixMinigame.SequenceElements
{
    public abstract class MixGameSequenceElement
    {
        protected readonly int     visualNumber;
        protected readonly float   appearTiming;
        protected readonly Vector2 initialPosition;
        
        public int     VisualNumber    => visualNumber;
        public float   AppearTiming    => appearTiming;
        public Vector2 InitialPosition => initialPosition;
        
        protected MixGameSequenceElement(int visualNumber, float appearTiming, Vector2 initialPosition)
        {
            this.visualNumber    = visualNumber;
            this.appearTiming    = appearTiming;
            this.initialPosition = initialPosition;
        }
    }
}