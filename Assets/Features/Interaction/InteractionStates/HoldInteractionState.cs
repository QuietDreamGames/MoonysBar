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
    public class HoldInteractionState : IState, IUpdateHandler
    {
        private readonly IMachine                          _stateMachine;
        private readonly IInputEventBusFeed                _inputEventBusFeed;
        private readonly InteractionHitRegistrator         _hitRegistrator;
        private readonly InteractionPointerCollisionBuffer _collisionBuffer;
        private readonly IInteractionEventBusSink          _interactionEventBusSink;

        private bool _isActive;

        private readonly PointerCollider[] _internalTargetCollidersBuffer = new PointerCollider[10];

        public HoldInteractionState(
            IMachine                          stateMachine,
            IInputEventBusFeed                inputEventBusFeed,
            InteractionHitRegistrator         hitRegistrator,
            InteractionPointerCollisionBuffer collisionBuffer,
            IInteractionEventBusSink          interactionEventBusSink
        )
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

            _collisionBuffer.Clear();

            var hitsDetected = _hitRegistrator.TryHitAllPointerColliders(out var pointerColliders);
            _collisionBuffer.UpdateBuffer(pointerColliders);

            _inputEventBusFeed.OnPointerHoldEnd += OnPointerHoldEnd;
            _inputEventBusFeed.OnPointerDrag    += OnPointerDrag;

            if (!hitsDetected) return;

            if (_interactionEventBusSink.SupportsMultipleHits)
                _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                    kind: InteractionKind.Hold,
                    phase: InteractionPhase.Start,
                    targets: pointerColliders.AsMemory()
                ));
            else
                _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                    kind: InteractionKind.Hold,
                    phase: InteractionPhase.Start,
                    target: pointerColliders[0]
                ));
        }

        public void Exit()
        {
            _isActive = false;

            _inputEventBusFeed.OnPointerHoldEnd -= OnPointerHoldEnd;
            _inputEventBusFeed.OnPointerDrag    -= OnPointerDrag;
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
                        _internalTargetCollidersBuffer[i] = newCollidersSpan[index];
                    }

                    _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                        kind: InteractionKind.Hold,
                        phase: InteractionPhase.Collect,
                        targets: _internalTargetCollidersBuffer.AsMemory(start: 0, length: targetsCount)
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
                        _internalTargetCollidersBuffer[i] = oldCollidersSpan[index];
                    }

                    _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                        kind: InteractionKind.Hold,
                        phase: InteractionPhase.PrematureExit,
                        targets: _internalTargetCollidersBuffer.AsMemory(start: 0, length: targetsCount)
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
                                kind: InteractionKind.Hold,
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
                                kind: InteractionKind.Hold,
                                phase: InteractionPhase.PrematureExit,
                                target: _collisionBuffer.CollidersBuffer[0]
                            ));

                        if (_interactionEventBusSink.SupportsCollects)
                            _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                                kind: InteractionKind.Hold,
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
                                kind: InteractionKind.Hold,
                                phase: InteractionPhase.PrematureExit,
                                target: _collisionBuffer.CollidersBuffer[0]
                            ));
                        break;
                    }
                }
            }

            _collisionBuffer.UpdateBuffer(newCollidersArray);
        }

        private void OnPointerHoldEnd()
        {
            _stateMachine.Enter<IdleInteractionState>();

            if (_interactionEventBusSink.SupportsMultipleHits)
            {
                if (!_hitRegistrator.TryHitAllPointerColliders(out var pointerColliders)) return;

                _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                    kind: InteractionKind.Hold,
                    phase: InteractionPhase.End,
                    targets: pointerColliders
                ));
            }
            else if (_hitRegistrator.TryHitPointerCollider(out var pointerCollider))
            {
                _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                    kind: InteractionKind.Hold,
                    phase: InteractionPhase.End,
                    target: pointerCollider
                ));
            }
        }

        private void OnPointerDrag(Vector2 delta)
        {
            // buffer is supposed to be shared. drag state cant realistically get delta so that's the only way.
            _hitRegistrator.TryHitAllPointerCollidersWithDelta(delta: -delta, colliders: out var pointerColliders);
            _collisionBuffer.UpdateBuffer(pointerColliders);

            _stateMachine.Enter<DragInteractionState>();

            if (_collisionBuffer.IsEmpty(1)) return;
            if (_interactionEventBusSink.SupportsMultipleHits)
                _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                    kind: InteractionKind.Drag,
                    phase: InteractionPhase.Action,
                    targets: _collisionBuffer.CollidersBuffer.AsMemory(start: 0, length: _collisionBuffer.FindLength()),
                    delta: delta
                ));
            else
                _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                    kind: InteractionKind.Drag,
                    phase: InteractionPhase.Action,
                    target: _collisionBuffer.CollidersBuffer[0],
                    delta: delta
                ));
        }
    }
}
