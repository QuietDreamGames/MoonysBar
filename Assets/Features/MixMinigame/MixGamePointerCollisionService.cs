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

            if (_tilesHolderAndUpdater.TryFindTileByPresenter(presenter: tileClickableView, result: out var tile))
                tile.Item3.HandleInteraction(_levelTimerHolder.Timer);
        }

        private void OnPointerColliderHeld(PointerCollider pointerCollider, bool isHeld)
        {
            if (pointerCollider is not MixGamePointerCollider mixGamePointerCollider) return;
            if (mixGamePointerCollider.IsClickable) return;

            var tileMovableView = pointerCollider.GetComponentInParent<MixGameTileMovableView>();

            if (_tilesHolderAndUpdater.TryFindTileByPresenter(presenter: tileMovableView, result: out var tile))
                tile.Item3.HandleInteraction(levelTimerValue: _levelTimerHolder.Timer, isHeld: isHeld);
        }
    }
}
