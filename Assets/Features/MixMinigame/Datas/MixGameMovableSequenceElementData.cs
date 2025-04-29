using UnityEngine;

namespace Features.MixMinigame.Datas
{
    public class MixGameMovableSequenceElementData : MixGameSequenceElementData
    {
        protected readonly float     moveDuration;
        protected readonly Vector2   finalPosition;
        protected readonly Vector2[] movePath;
        
        public float     MoveDuration  => moveDuration;
        public Vector2   FinalPosition => finalPosition;
        public Vector2[] MovePath      => movePath;
        
        public MixGameMovableSequenceElementData(
            int     visualNumber,
            float   appearTiming,
            Vector2 initialPosition,
            Vector2 finalPosition,
            float   moveDuration,
            Vector2[] movePath
            ) : base(visualNumber, appearTiming, initialPosition)
        {
            this.finalPosition = finalPosition;
            this.moveDuration  = moveDuration;
            this.movePath      = movePath;
        }
    }
}