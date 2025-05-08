using Features.MixMinigame.Models;

namespace Features.MixMinigame.ViewModels
{
    public class MixGameTileMovableViewModel : MixGameTileViewModel
    {
        private bool _isProcessing;

        public MixGameTileMovableViewModel(MixGameTileModel tileModel) : base(tileModel)
        {
        }

        public override void CheckForMiss(float levelTimerValue)
        {
            if (IsProcessed) return;

            if (!_isProcessing && TileModel.IsMissedStart(levelTimerValue))
            {
                IsProcessed = true;
                TriggerMiss();
                return;
            }

            if (_isProcessing && ((MixGameTileMovableModel)TileModel).IsMissedFinish(levelTimerValue))
            {
                IsProcessed = true;
                TriggerMiss();
            }
        }

        public override void HandleInteraction(float levelTimerValue, bool isHeld = false)
        {
            if (IsProcessed) return;

            if (isHeld)
            {
                if (_isProcessing) return;
                _isProcessing = true;
                TriggerHit();
                return;
            }

            // is held here is false
            if (!_isProcessing) return;
            _isProcessing = false;
            IsProcessed   = true;

            var tileModel = (MixGameTileMovableModel)TileModel;

            if (tileModel.IsMissedFinish(levelTimerValue))
                TriggerFail();
            else
                TriggerHit();
        }
    }
}