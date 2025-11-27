using System;
using Features.FiniteStateMachine.Interfaces;
using Features.Input.Interfaces;
using Features.Interaction.Enums;
using Features.Interaction.Helpers;
using Features.Interaction.Interfaces;
using UnityEngine;

namespace Features.Interaction.InteractionStates
{
    public class IdleInteractionState : IState
    {
        private readonly IMachine                          _stateMachine;
        private readonly IInputEventBusFeed                _inputEventBusFeed;
        private readonly InteractionHitRegistrator         _hitRegistrator;
        private readonly InteractionPointerCollisionBuffer _collisionBuffer;
        private readonly IInteractionEventBusSink          _interactionEventBusSink;

        public IdleInteractionState(
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
            _inputEventBusFeed.OnPointerClick     += OnPointerClicked;
            _inputEventBusFeed.OnPointerHoldStart += OnPointerHoldStarted;
            _inputEventBusFeed.OnPointerDrag      += OnPointerDragged;
        }

        public void Exit()
        {
            _inputEventBusFeed.OnPointerClick     -= OnPointerClicked;
            _inputEventBusFeed.OnPointerHoldStart -= OnPointerHoldStarted;
            _inputEventBusFeed.OnPointerDrag      -= OnPointerDragged;
        }

        private void OnPointerClicked()
        {
            if (_interactionEventBusSink.SupportsMultipleHits)
            {
                if (_hitRegistrator.TryHitAllPointerColliders(out var pointerColliders))
                    _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                        kind: InteractionKind.Click,
                        phase: InteractionPhase.Action,
                        targets: pointerColliders.AsMemory()
                    ));
            }
            else if (_hitRegistrator.TryHitPointerCollider(out var pointerCollider))
            {
                _interactionEventBusSink.HandleInteraction(new InteractionEvent(
                    kind: InteractionKind.Click,
                    phase: InteractionPhase.Action,
                    target: pointerCollider
                ));
            }
        }

        private void OnPointerHoldStarted()
        {
            _stateMachine.Enter<HoldInteractionState>();
        }

        private void OnPointerDragged(Vector2 delta)
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
