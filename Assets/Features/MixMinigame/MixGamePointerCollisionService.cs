using System;
using Features.Interaction;
using Features.Interaction.Enums;
using Features.Interaction.Interfaces;
using Features.MixMinigame.Views;
using JetBrains.Annotations;
using VContainer;

namespace Features.MixMinigame
{
    public class MixGamePointerCollisionService : IDisposable
    {
        private readonly MixGameLevelTimerHolder      _levelTimerHolder;
        private readonly MixGameTilesHolderAndUpdater _tilesHolderAndUpdater;

        private          Action<InteractionEvent> _onPointerColliderEvent;
        private readonly IDisposable              _subscriptionDisposable;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public MixGamePointerCollisionService(
            IInteractionEventBusFeed     interactionEventBusFeed,
            MixGameTilesHolderAndUpdater tilesHolderAndUpdater,
            MixGameLevelTimerHolder      levelTimerHolder)
        {
            _onPointerColliderEvent = OnPointerColliderEvent;

            _subscriptionDisposable = interactionEventBusFeed.Subscribe(
                kinds: InteractionKind.Click | InteractionKind.Drag | InteractionKind.Hold,
                phases: InteractionPhase.Action | InteractionPhase.Start | InteractionPhase.End,
                supportsMultipleHits: false,
                handler: _onPointerColliderEvent
            );

            _tilesHolderAndUpdater = tilesHolderAndUpdater;
            _levelTimerHolder      = levelTimerHolder;
        }

        public void Dispose()
        {
            _onPointerColliderEvent = null;
            _subscriptionDisposable.Dispose();
        }

        private void OnPointerColliderEvent(InteractionEvent interactionEvent)
        {
            switch (interactionEvent.Kind)
            {
                case InteractionKind.Click:
                    if (interactionEvent.Phase == InteractionPhase.Action)
                        HandleColliderClickAction(interactionEvent);
                    break;
                case InteractionKind.Hold:
                    HandleColliderHold(interactionEvent);
                    break;
                case InteractionKind.Drag:
                    HandleColliderDrag(interactionEvent);
                    break;
            }
        }

        private void HandleColliderClickAction(InteractionEvent interactionEvent)
        {
            if (!interactionEvent.TryGetFirstTargetOfType<MixGamePointerCollider>(out var pointerCollider))
                return;

            if (!pointerCollider.IsClickable) return;
            var tileClickableView = pointerCollider.GetComponentInParent<MixGameTileClickableView>();

            if (_tilesHolderAndUpdater.TryFindTileByPresenter(presenter: tileClickableView, result: out var tile))
                tile.Item3.HandleInteraction(_levelTimerHolder.Timer);
        }

        private void HandleColliderHold(InteractionEvent interactionEvent)
        {
            if (!interactionEvent.TryGetFirstTargetOfType<MixGamePointerCollider>(out var pointerCollider))
                return;

            if (pointerCollider.IsClickable) return;
            var tileMovableView = pointerCollider.GetComponentInParent<MixGameTileMovableView>();

            if (!_tilesHolderAndUpdater.TryFindTileByPresenter(presenter: tileMovableView, result: out var tile))
                return;
            bool isHeld;
            switch (interactionEvent.Phase)
            {
                case InteractionPhase.Start:
                    isHeld = true;
                    break;
                case InteractionPhase.End:
                    isHeld = false;
                    break;
                default:
                    return;
            }

            tile.Item3.HandleInteraction(levelTimerValue: _levelTimerHolder.Timer, isHeld: isHeld);
        }

        private void HandleColliderDrag(InteractionEvent interactionEvent)
        {
            if (!interactionEvent.TryGetFirstTargetOfType<MixGamePointerCollider>(out var pointerCollider))
                return;

            if (pointerCollider.IsClickable) return;
            var tileMovableView = pointerCollider.GetComponentInParent<MixGameTileMovableView>();

            if (!_tilesHolderAndUpdater.TryFindTileByPresenter(presenter: tileMovableView, result: out var tile))
                return;
            bool isHeld;
            switch (interactionEvent.Phase)
            {
                case InteractionPhase.Start:
                    isHeld = true;
                    break;
                case InteractionPhase.End:
                    isHeld = false;
                    break;
                default:
                    return;
            }

            tile.Item3.HandleInteraction(levelTimerValue: _levelTimerHolder.Timer, isHeld: isHeld);
        }
    }
}
