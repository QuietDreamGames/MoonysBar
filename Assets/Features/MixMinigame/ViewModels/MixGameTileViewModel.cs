using System;
using Features.MixMinigame.Models;

namespace Features.MixMinigame.ViewModels
{
    public abstract class MixGameTileViewModel : IDisposable
    {
        protected bool IsProcessed;

        public MixGameTileViewModel(MixGameTileModel tileModel)
        {
            TileModel = tileModel;
        }

        public MixGameTileModel TileModel { get; }

        public void Dispose()
        {
            OnHit  = null;
            OnMiss = null;
            OnFail = null;
        }

        public event Action OnHit;
        public event Action OnMiss;
        public event Action OnFail;

        public abstract void CheckForMiss(float levelTimerValue);

        public abstract void HandleInteraction(float levelTimerValue, bool isHeld = false);

        protected void TriggerHit()
        {
            OnHit?.Invoke();
        }

        protected void TriggerMiss()
        {
            OnMiss?.Invoke();
        }

        protected void TriggerFail()
        {
            OnFail?.Invoke();
        }
    }
}
