using System;
using Features.Collision;
using Features.FiniteStateMachine.Interfaces;
using Features.Input.Interfaces;
using Features.Interaction.Enums;
using Features.Interaction.Helpers;
using Features.Interaction.Interfaces;
using Features.TimeSystem.Interfaces.Handlers;
using UnityEngine;

namespace Features.Interaction.InteractionStates
{
    public class DragInteractionState : IState, IUpdateHandler
    {
        private readonly IMachine                          _stateMachine;
        private readonly IInputEventBusFeed                _inputEventBusFeed;
        private readonly InteractionHitRegistrator         _hitRegistrator;
        private readonly InteractionPointerCollisionBuffer _collisionBuffer;
        private readonly IInteractionEventBusSink          _interactionEventBusSink;

        private bool _isActive;

        private readonly PointerCollider[] _sharedTargetCollidersBuffer = new PointerCollider[10];

        public DragInteractionState(
            IMachine                          stateMachine,
            IInputEventBusFeed                inputEventBusFeed,
            InteractionHitRegistrator         hitRegistrator,
            InteractionPointerCollisionBuffer collisionBuffer,
            IInteractionEventBusSink          interactionEventBusSink)
        {
            _stateMachine            = stateMachine;
            _inputEventBusFeed       = inputEventBusFeed;
            _hitRegistrator          = hitRegistrator;
            _collisionBuffer         = collisionBuffer;
            _interactionEventBusSink = interactionEventBusSink;
        }

        public void Enter()
        {
            _isActive = true;

            _inputEventBusFeed.OnPointerDrag    += OnPointerDrag;
            _inputEventBusFeed.OnPointerDragEnd += OnPointerDragEnd;
        }

        public void Exit()
        {
            _isActive = false;

            _inputEventBusFeed.OnPointerDrag    -= OnPointerDrag;
            _inputEventBusFeed.OnPointerDragEnd -= OnPointerDragEnd;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_isActive) return;
            if (!(_interactionEventBusSink.SupportsCollects || _interactionEventBusSink.SupportsPrematureExits)) return;

            var hitsDetected = _hitRegistrator.TryHitAllPointerColliders(out var newCollidersArray);
            if (!hitsDetected && _collisionBuffer.IsEmpty(1)) return;

            if (_interactionEventBusSink.SupportsMultipleHits)
            {
                var newCollidersSpan = newCollidersArray.AsSpan();
                var oldCollidersSpan = _collisionBuffer.CollidersBuffer.AsSpan();

                var colliderUpdates = _collisionBuffer.TryDetectUpdatedColliders(
                    newBuffer: newCollidersSpan,
                    oldBuffer: oldCollidersSpan);

                if (_interactionEventBusSink.SupportsCollects
                    && colliderUpdates.HasUpdates
                    && colliderUpdates.AddedIndices.Length > 0)
                {
                    var targetsCount = colliderUpdates.AddedIndices.Length;

                    for (var i = 0; i < targetsCount; i++)
                    {
                        var index = colliderUpdates.AddedIndices[i];
                        _sharedTargetCollidersBuffer[i] = newCollidersSpan[index];
                    }

                    _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                        kind: InteractionKind.Drag,
                        phase: InteractionPhase.Collect,
                        targets: _sharedTargetCollidersBuffer.AsSpan(start: 0, length: targetsCount)
                    ));
                }

                if (_interactionEventBusSink.SupportsPrematureExits
                    && colliderUpdates.HasUpdates
                    && colliderUpdates.RemovedIndices.Length > 0)
                {
                    var targetsCount = colliderUpdates.RemovedIndices.Length;

                    for (var i = 0; i < targetsCount; i++)
                    {
                        var index = colliderUpdates.RemovedIndices[i];
                        _sharedTargetCollidersBuffer[i] = oldCollidersSpan[index];
                    }

                    _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                        kind: InteractionKind.Drag,
                        phase: InteractionPhase.PrematureExit,
                        targets: _sharedTargetCollidersBuffer.AsSpan(start: 0, length: targetsCount)
                    ));
                }
            }
            else
            {
                switch (hitsDetected)
                {
                    // 1. there were no hit and a hit appeared:
                    case true when _collisionBuffer.IsEmpty(1):
                    {
                        if (_interactionEventBusSink.SupportsCollects)
                            _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                                kind: InteractionKind.Drag,
                                phase: InteractionPhase.Collect,
                                target: newCollidersArray[0]
                            ));
                        break;
                    }
                    // 2. there was a hit, and it is still there:
                    case true when !_collisionBuffer.IsEmpty(1):
                    {
                        if (_collisionBuffer.FindFirstCollider(newCollidersArray)) return;

                        if (_interactionEventBusSink.SupportsPrematureExits)
                            _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                                kind: InteractionKind.Drag,
                                phase: InteractionPhase.PrematureExit,
                                target: _collisionBuffer.CollidersBuffer[0]
                            ));

                        if (_interactionEventBusSink.SupportsCollects)
                            _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                                kind: InteractionKind.Drag,
                                phase: InteractionPhase.Collect,
                                target: newCollidersArray[0]
                            ));
                        break;
                    }
                    // 3. there was a hit and now there is none:
                    case false when !_collisionBuffer.IsEmpty(1):
                    {
                        if (_interactionEventBusSink.SupportsPrematureExits)
                            _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                                kind: InteractionKind.Drag,
                                phase: InteractionPhase.PrematureExit,
                                target: _collisionBuffer.CollidersBuffer[0]
                            ));
                        break;
                    }
                }
            }

            _collisionBuffer.UpdateBuffer(newCollidersArray);
        }

        private void OnPointerDragEnd()
        {
            if (_interactionEventBusSink.SupportsMultipleHits)
            {
                if (_hitRegistrator.TryHitAllPointerColliders(out var pointerColliders))
                    _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                        kind: InteractionKind.Drag,
                        phase: InteractionPhase.End,
                        targets: pointerColliders.AsSpan()
                    ));
            }
            else if (_hitRegistrator.TryHitPointerCollider(out var pointerCollider))
            {
                _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                    kind: InteractionKind.Drag,
                    phase: InteractionPhase.End,
                    target: pointerCollider
                ));
            }

            _stateMachine.Enter<IdleInteractionState>();
        }

        private void OnPointerDrag(Vector2 delta)
        {
            if (_collisionBuffer.IsEmpty(1)) return;
            if (_interactionEventBusSink.SupportsMultipleHits)
                _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                    kind: InteractionKind.Drag,
                    phase: InteractionPhase.Action,
                    targets: _collisionBuffer.CollidersBuffer.AsSpan(start: 0, length: _collisionBuffer.FindLength()),
                    delta: delta
                ));
            else
                _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                    kind: InteractionKind.Drag,
                    phase: InteractionPhase.Action,
                    target: _collisionBuffer.CollidersBuffer[0],
                    delta: delta
                ));


            // SUGGESTION: there is a chance we want collects and exits to be assumed before the update was triggered.
            //             in this case, we need to manage it here in a similar manner with update.
        }
    }
}
