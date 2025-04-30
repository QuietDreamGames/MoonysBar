using System;
using Features.MixMinigame.Models;

namespace Features.MixMinigame.ViewModels
{
    public abstract class MixGameTileViewModel : IDisposable
    {
        public MixGameTileModel TileModel { get; }

        public event Action OnHit;
        public event Action OnMiss;
        public event Action OnFail;

        protected bool IsProcessed;
        
        public MixGameTileViewModel(MixGameTileModel tileModel)
        {
            TileModel = tileModel;
        }

        public abstract void HandleInteraction(bool isHeld = false);

        public void Dispose()
        {
            OnHit = null;
            OnMiss = null;
            OnFail = null;
        }
    }
}