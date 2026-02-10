using Features.MixMinigame.Models;

namespace Features.MixMinigame.Views
{
    public class MixGameTileClickableView : MixGameTileView
    {
        public MixGameTileClickableView(MixGameTileModel tileModel) : base(tileModel)
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
