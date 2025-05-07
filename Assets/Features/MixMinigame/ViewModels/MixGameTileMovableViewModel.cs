using Features.MixMinigame.Models;

namespace Features.MixMinigame.ViewModels
{
    public class MixGameTileMovableViewModel : MixGameTileViewModel
    {
        public MixGameTileMovableViewModel(MixGameTileModel tileModel) : base(tileModel)
        {
        }

        public override void HandleInteraction(bool isHeld = false)
        {
        }
    }
}