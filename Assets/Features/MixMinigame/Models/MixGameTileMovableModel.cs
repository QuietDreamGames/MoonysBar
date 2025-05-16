using System;
using Features.MixMinigame.Datas;

namespace Features.MixMinigame.Models
{
    public class MixGameTileMovableModel : MixGameTileModel
    {
        public MixGameTileMovableModel(MixGameSequenceElementData data, float hitTiming, float forgivenessWindow)
            : base(data, hitTiming, forgivenessWindow)
        {
            if (data is not MixGameMovableSequenceElementData)
                throw new ArgumentException("Data must be of type MixGameMovableSequenceElementData");
        }

        public bool IsReleasedInForgivenessWindow(float levelTimerValue)
        {
            var movableData = (MixGameMovableSequenceElementData)Data;
            return levelTimerValue >= Data.AppearTiming + HitTiming + movableData.MoveDuration - ForgivenessWindow &&
                   levelTimerValue <= Data.AppearTiming + HitTiming + movableData.MoveDuration + ForgivenessWindow;
        }

        public bool IsMissedFinish(float levelTimerValue)
        {
            var movableData = (MixGameMovableSequenceElementData)Data;
            return levelTimerValue > Data.AppearTiming + HitTiming + movableData.MoveDuration + ForgivenessWindow;
        }
    }
}
