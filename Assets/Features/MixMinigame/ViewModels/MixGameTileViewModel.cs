using System;
using Features.MixMinigame.Models;
using Features.TimeSystem.Interfaces.Handlers;

namespace Features.MixMinigame.ViewModels
{
    public abstract class MixGameTileViewModel : IUpdateHandler
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
        
        public virtual void OnUpdate(float deltaTime)
        {
            
        }
    }
}