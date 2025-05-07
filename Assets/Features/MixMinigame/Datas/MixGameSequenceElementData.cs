using UnityEngine;

namespace Features.MixMinigame.Datas
{
    public abstract class MixGameSequenceElementData
    {
        protected readonly int     visualNumber;
        protected readonly float   appearTiming;
        protected readonly Vector2 initialPosition;

        public int     VisualNumber    => visualNumber;
        public float   AppearTiming    => appearTiming;
        public Vector2 InitialPosition => initialPosition;

        protected MixGameSequenceElementData(int visualNumber, float appearTiming, Vector2 initialPosition)
        {
            this.visualNumber    = visualNumber;
            this.appearTiming    = appearTiming;
            this.initialPosition = initialPosition;
        }
    }
}