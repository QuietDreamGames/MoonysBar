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

            if (!TileModel.IsMissedStart(levelTimerValue)) return;

            IsProcessed = true;
            TriggerMiss();
        }

        public override void HandleInteraction(float levelTimerValue, bool isHeld = false)
        {
            if (IsProcessed) return;

            IsProcessed = true;

            if (TileModel.IsHitInForgivenessWindow(levelTimerValue))
                TriggerHit();
            else
                TriggerFail();
        }
    }
}
