using Features.MixMinigame.Datas;

namespace Features.MixMinigame.Models
{
    public abstract class MixGameTileModel
    {
        public MixGameTileModel(MixGameSequenceElementData data, float hitTiming, float forgivenessWindow)
        {
            Data              = data;
            HitTiming         = hitTiming;
            ForgivenessWindow = forgivenessWindow;
        }

        public MixGameSequenceElementData Data { get; }

        public float HitTiming         { get; }
        public float ForgivenessWindow { get; }

        public bool IsHitInForgivenessWindow(float levelTimerValue)
        {
            return levelTimerValue >= Data.AppearTiming + HitTiming - ForgivenessWindow
                   && levelTimerValue <= Data.AppearTiming + HitTiming + ForgivenessWindow;
        }

        public bool IsMissedStart(float levelTimerValue)
        {
            return levelTimerValue > Data.AppearTiming + HitTiming + ForgivenessWindow;
        }
    }
}
