using Features.MixMinigame.Models;

namespace Features.MixMinigame.ViewModels
{
    public class MixGameTileClickableViewModel : MixGameTileViewModel
    {
        public MixGameTileClickableViewModel(MixGameTileModel tileModel) : base(tileModel)
        {
        }

        public override void CheckForMiss(float levelTimerValue)
        {
            if (IsProcessed) return;

            if (TileModel.IsMissedStart(levelTimerValue))
            {
                IsProcessed = true;
                TriggerMiss();
            }
        }

        public override void HandleInteraction(float levelTimerValue, bool isHeld = false)
        {
            IsProcessed = true;
            TriggerHit();
        }
    }
}