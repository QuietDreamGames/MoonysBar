using Features.MixMinigame.Datas;

namespace Features.MixMinigame.Models
{
    public class MixGameTileMovableModel : MixGameTileModel
    {
        public MixGameTileMovableModel(MixGameSequenceElementData data, float forgivenessWindow)
            : base(data, forgivenessWindow)
        {
        }

        public bool IsFinishedInForgivenessWindow(float time)
        {
            if (Data is not MixGameMovableSequenceElementData movableData) return false;

            return time >= Data.AppearTiming + movableData.MoveDuration - ForgivenessWindow &&
                   time <= Data.AppearTiming + movableData.MoveDuration + ForgivenessWindow;
        }
    }
}