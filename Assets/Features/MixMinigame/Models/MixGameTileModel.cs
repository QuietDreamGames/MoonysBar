using Features.MixMinigame.Datas;

namespace Features.MixMinigame.Models
{
    public abstract class MixGameTileModel
    {
        public MixGameSequenceElementData Data { get; }

        public float ForgivenessWindow { get; }


        public MixGameTileModel(MixGameSequenceElementData data, float forgivenessWindow)
        {
            Data              = data;
            ForgivenessWindow = forgivenessWindow;
        }
        
        public bool IsInForgivenessWindow(float time)
        {
            return time >= Data.AppearTiming - ForgivenessWindow && time <= Data.AppearTiming + ForgivenessWindow;
        }
    }
}