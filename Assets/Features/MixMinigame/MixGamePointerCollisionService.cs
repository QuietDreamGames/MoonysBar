using Features.Collision;
using Features.InputDispatching;
using Features.MixMinigame.Views;
using JetBrains.Annotations;
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
            InputPointerCollisionService inputPointerCollisionService,
            MixGameTilesHolderAndUpdater tilesHolderAndUpdater,
            MixGameLevelTimerHolder      levelTimerHolder)
        {
            inputPointerCollisionService.OnClickedPointerColliderAction += OnPointerColliderClicked;
            inputPointerCollisionService.OnHeldPointerColliderAction    += OnPointerColliderHeld;

            _tilesHolderAndUpdater = tilesHolderAndUpdater;
            _levelTimerHolder      = levelTimerHolder;
        }

        private void OnPointerColliderClicked(PointerCollider pointerCollider)
        {
            if (pointerCollider is not MixGamePointerCollider mixGamePointerCollider) return;
            if (!mixGamePointerCollider.IsClickable) return;

            var tileClickableView = pointerCollider.GetComponentInParent<MixGameTileClickableView>();

            if (!tileClickableView) return;

            var tiles = _tilesHolderAndUpdater.GetTiles();
            for (var i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].Item2 != tileClickableView) continue;
                tiles[i].Item3.HandleInteraction(_levelTimerHolder.Timer);
                break;
            }
        }

        private void OnPointerColliderHeld(PointerCollider pointerCollider, bool isHeld)
        {
            if (pointerCollider is not MixGamePointerCollider mixGamePointerCollider) return;
            if (mixGamePointerCollider.IsClickable) return;

            var tileMovableView = pointerCollider.GetComponentInParent<MixGameTileMovableView>();
            if (!tileMovableView) return;

            var tiles = _tilesHolderAndUpdater.GetTiles();
            for (var i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].Item2 != tileMovableView) continue;
                tiles[i].Item3.HandleInteraction(_levelTimerHolder.Timer, isHeld);
                break;
            }
        }
    }
}
