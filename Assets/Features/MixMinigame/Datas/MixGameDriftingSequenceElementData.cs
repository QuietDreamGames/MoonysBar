using UnityEngine;

namespace Features.MixMinigame.Datas
{
    public class MixGameDriftingSequenceElementData : MixGameClickableSequenceElementData
    {
        public MixGameDriftingSequenceElementData(
            int     visualNumber,
            float   appearTiming,
            Vector2 initialPosition,
            float   driftFinalPositionY
        ) :
            base(visualNumber, appearTiming, initialPosition)
        {
            DriftFinalPositionY = driftFinalPositionY;
        }

        public float DriftFinalPositionY { get; }
    }
}
