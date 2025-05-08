using Features.InputDispatching;
using Features.MixMinigame.Views;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Features.MixMinigame
{
    public class MixGamePointerCollisionService
    {
        private readonly MixGameLevelTimerHolder      _levelTimerHolder;
        private readonly MixGameTilesHolderAndUpdater _tilesHolderAndUpdater;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public MixGamePointerCollisionService(
            InputPointerGameObjectsCollisionService inputPointerGameObjectCollisionService,
            MixGameTilesHolderAndUpdater            tilesHolderAndUpdater,
            MixGameLevelTimerHolder                 levelTimerHolder)
        {
            inputPointerGameObjectCollisionService.OnClickedGameObjectAction += OnGameObjectClicked;
            inputPointerGameObjectCollisionService.OnHeldGameObjectAction    += OnGameObjectHeld;

            _tilesHolderAndUpdater = tilesHolderAndUpdater;
            _levelTimerHolder      = levelTimerHolder;
        }

        private void OnGameObjectClicked(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out MixGameTileClickableView mixGameTileClickableView)) return;

            var tiles = _tilesHolderAndUpdater.GetTiles();
            for (var i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].Item2 != mixGameTileClickableView) continue;
                tiles[i].Item3.HandleInteraction(_levelTimerHolder.Timer);
                break;
            }
        }

        private void OnGameObjectHeld(GameObject gameObject, bool isHeld)
        {
            if (!gameObject.TryGetComponent(out MixGameTileMovableView mixGameTileMovableView)) return;

            var tiles = _tilesHolderAndUpdater.GetTiles();
            for (var i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].Item2 != mixGameTileMovableView) continue;
                tiles[i].Item3.HandleInteraction(_levelTimerHolder.Timer, isHeld);
                break;
            }
        }
    }
}