using Features.MixMinigame.Datas;

namespace Features.MixMinigame.Models
{
    public abstract class MixGameTileModel
    {
        public MixGameTileModel(MixGameSequenceElementData data, float forgivenessWindow)
        {
            Data              = data;
            ForgivenessWindow = forgivenessWindow;
        }

        public MixGameSequenceElementData Data { get; }

        public float ForgivenessWindow { get; }

        public bool IsHitInForgivenessWindow(float levelTimerValue)
        {
            return levelTimerValue >= Data.AppearTiming - ForgivenessWindow
                   && levelTimerValue <= Data.AppearTiming + ForgivenessWindow;
        }

        public bool IsMissedStart(float levelTimerValue)
        {
            return levelTimerValue > Data.AppearTiming + ForgivenessWindow;
        }
    }
}