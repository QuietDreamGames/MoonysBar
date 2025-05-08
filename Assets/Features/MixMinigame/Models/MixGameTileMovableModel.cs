using System;
using Features.MixMinigame.Datas;

namespace Features.MixMinigame.Models
{
    public class MixGameTileMovableModel : MixGameTileModel
    {
        public MixGameTileMovableModel(MixGameSequenceElementData data, float forgivenessWindow)
            : base(data, forgivenessWindow)
        {
            if (data is not MixGameMovableSequenceElementData)
                throw new ArgumentException("Data must be of type MixGameMovableSequenceElementData");
        }

        public bool IsReleasedInForgivenessWindow(float levelTimerValue)
        {
            var movableData = (MixGameMovableSequenceElementData)Data;
            return levelTimerValue >= Data.AppearTiming + movableData.MoveDuration - ForgivenessWindow &&
                   levelTimerValue <= Data.AppearTiming + movableData.MoveDuration + ForgivenessWindow;
        }

        public bool IsMissedFinish(float levelTimerValue)
        {
            var movableData = (MixGameMovableSequenceElementData)Data;
            return levelTimerValue > Data.AppearTiming + movableData.MoveDuration + ForgivenessWindow;
        }
    }
}