using Features.MixMinigame.Datas;

namespace Features.MixMinigame.Models
{
    public class MixGameTileClickableModel : MixGameTileModel
    {
        public MixGameTileClickableModel(MixGameSequenceElementData data, float hitTiming, float forgivenessWindow)
            : base(data, hitTiming, forgivenessWindow)
        {
        }
    }
}
