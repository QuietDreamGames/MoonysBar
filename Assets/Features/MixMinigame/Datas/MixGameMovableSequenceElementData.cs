using UnityEngine;

namespace Features.MixMinigame.Datas
{
    public class MixGameMovableSequenceElementData : MixGameSequenceElementData
    {
        protected readonly float moveDuration;
        protected readonly float rotationZEuler;
        protected readonly int   tileType;

        public MixGameMovableSequenceElementData(
            int     visualNumber,
            float   appearTiming,
            Vector2 initialPosition,
            float   rotationZEuler,
            float   moveDuration,
            int     tileType
        ) : base(visualNumber, appearTiming, initialPosition)
        {
            this.rotationZEuler = rotationZEuler;
            this.moveDuration   = moveDuration;
            this.tileType       = tileType;
        }

        public float MoveDuration   => moveDuration;
        public float RotationZEuler => rotationZEuler;
        public int   TileType       => tileType;
    }
}
