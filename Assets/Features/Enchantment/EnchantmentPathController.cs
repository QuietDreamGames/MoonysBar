using System;
using System.Collections.Generic;
using Features.CameraSystem;
using Features.Enchantment.Datas;
using Features.Enchantment.Enums;
using Features.Enchantment.Presenters;
using Features.Input;
using Features.Interaction;
using Features.Interaction.Enums;
using Features.Interaction.Interfaces;
using Features.TimeSystem.Interfaces.Handlers;
using UnityEngine;
using VContainer;

namespace Features.Enchantment
{
    public class EnchantmentPathController : IDisposable, IUpdateHandler
    {
        private readonly CameraHolderService                 _cameraHolderService;
        private readonly EnchantmentElementsHolderAndUpdater _elementsHolder;
        private readonly IDisposable                         _subscriptionDisposable;

        private List<NodeConnection> _currentConnections;
        private Stack<int>           _currentPath;

        private EnchantmentGraphData _layout;

        private Action<InteractionEvent> _onPointerColliderEvent;

        private Stack<TempNodeConnectionLine> _tempConnectionLines;

        [Inject]
        public EnchantmentPathController(
            IInteractionEventBusFeed            interactionEventBusFeed,
            EnchantmentElementsHolderAndUpdater elementsHolder,
            CameraHolderService                 cameraHolderService
        )
        {
            _elementsHolder      = elementsHolder;
            _cameraHolderService = cameraHolderService;

            _onPointerColliderEvent = OnPointerColliderEvent;

            _subscriptionDisposable = interactionEventBusFeed.Subscribe(
                kinds: InteractionKind.Drag | InteractionKind.Hold,
                phases: InteractionPhase.Action |
                        InteractionPhase.Start | InteractionPhase.End |
                        InteractionPhase.Collect | InteractionPhase.PrematureExit,
                supportsMultipleHits: true,
                handler: _onPointerColliderEvent
            );

            _tempConnectionLines = new Stack<TempNodeConnectionLine>();
        }

        public void Dispose()
        {
            _onPointerColliderEvent = null;
            _subscriptionDisposable.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_tempConnectionLines != null)
                foreach (var tempLine in _tempConnectionLines)
                    Debug.DrawLine(
                        start: tempLine.StartPosition,
                        end: tempLine.EndPosition,
                        color: Color.green
                    );

            if (!_elementsHolder.TryGetEnchantmentHandle(out var handle)) return;
            if (!InputUtils.TryGetPrimaryPointerScreenPosition(out var screenPosition)) return;
            if (!handle.Item2.IsHeld()) return;

            var cameraPosition = _cameraHolderService.MainCamera.ScreenToWorldPoint(screenPosition);
            var worldPosition  = new Vector2(x: cameraPosition.x, y: cameraPosition.y);
            handle.Item1.transform.position = worldPosition;


            var currentTempLine = _tempConnectionLines.Peek();
            currentTempLine.UpdateEndPosition(newEndPosition: worldPosition);
        }

        public void SetLayout(EnchantmentGraphData layout)
        {
            _layout = layout;

            _currentConnections  = new List<NodeConnection>(_layout.Connections);
            _currentPath         = new Stack<int>();
            _tempConnectionLines = new Stack<TempNodeConnectionLine>();

            if (_elementsHolder.TryGetEnchantmentHandle(out var handle))
                handle.Item2.Deactivate();
            else
                Debug.LogError("Enchantment Handle not found in Elements Holder.");
        }

        private void OnPointerColliderEvent(InteractionEvent interactionEvent)
        {
            if (_layout == null) return;

            if (!interactionEvent.IsMultiple)
            {
                Debug.LogError("Single pointer collider hit was fired, needs to be investigated");
                return;
            }

            if (interactionEvent is { Phase: InteractionPhase.Start, Kind: InteractionKind.Hold })
                HandleCollidersHoldStart(interactionEvent);
            if (interactionEvent is { Phase: InteractionPhase.Collect, Kind: InteractionKind.Hold })
                HandleColliderHoldCollect(interactionEvent);
            if (interactionEvent is { Phase: InteractionPhase.Collect, Kind: InteractionKind.Drag })
                HandleColliderDragCollect(interactionEvent);
            if (interactionEvent is { Phase: InteractionPhase.End, Kind: InteractionKind.Hold })
                HandleColliderHoldOrDragEnd(interactionEvent);
            if (interactionEvent is { Phase: InteractionPhase.End, Kind: InteractionKind.Drag })
                HandleColliderHoldOrDragEnd(interactionEvent);
        }

        private void HandleCollidersHoldStart(InteractionEvent interactionEvent)
        {
            // no node was connected yet. fresh start.
            if (_currentPath.Count == 0)
            {
                if (!interactionEvent.TryGetFirstTargetOfType<EnchantmentPointerCollider>(out var enchantmentCollider))
                    return;
                if (enchantmentCollider.PointerType != EnchantmentPointerColliderType.Node)
                {
                    Debug.LogError("Unexpected, any other ench collider shouldn't exist at this stage.");
                    return;
                }

                if (!TryGetNodeIndexByCollider(collider: enchantmentCollider, nodeIndex: out var nodeIndex)) return;
                if (!_elementsHolder.TryFindEnchantmentNodeByIndex(index: nodeIndex, result: out var nodeData)) return;

                nodeData.Item3.SetState(EnchantmentNodeViewState.UnconnectedHeld);
                _elementsHolder.TryGetEnchantmentHandle(out var handle);
                handle.Item2.Activate();

                if (InputUtils.TryGetPrimaryPointerScreenPosition(out var screenPosition))
                {
                    var cameraPosition = _cameraHolderService.MainCamera.ScreenToWorldPoint(screenPosition);
                    var worldPosition  = new Vector2(x: cameraPosition.x, y: cameraPosition.y);
                    handle.Item1.transform.position = worldPosition;
                }

                _currentPath.Push(nodeIndex);
            }

            // at least two nodes are now connected. handle is on the second node, this node state is 'ConnectedUnheld'
            if (_currentPath.Count >= 2)
            {
                EnchantmentPointerCollider handleCollider = null;
                EnchantmentPointerCollider nodeCollider   = null;

                foreach (var collider in interactionEvent.Targets.Span)
                {
                    if (!collider.TryGetComponent<EnchantmentPointerCollider>(out var enchantmentCollider)) continue;
                    switch (enchantmentCollider.PointerType)
                    {
                        case EnchantmentPointerColliderType.Handle:
                            handleCollider = enchantmentCollider;
                            break;
                        case EnchantmentPointerColliderType.Node:
                            nodeCollider = enchantmentCollider;
                            break;
                    }
                }

                if (handleCollider == null || nodeCollider == null) return;
                if (!_elementsHolder.TryGetEnchantmentHandle(out var handle)) return;
                if (!TryGetNodeIndexByCollider(collider: nodeCollider, nodeIndex: out var nodeIndex)) return;

                var lastConnectedNodeId = _currentPath.Peek();

                if (nodeIndex != lastConnectedNodeId) return;

                if (!_elementsHolder.TryFindEnchantmentNodeByIndex(index: nodeIndex, result: out var node)) return;

                handle.Item2.HandleHold(true);
                node.Item3.SetState(EnchantmentNodeViewState.ConnectedHeld);

                _tempConnectionLines.Push(new TempNodeConnectionLine(
                                              startPosition: node.Item2.transform.position,
                                              endPosition: handle.Item1.transform.position
                                          ));
            }
        }

        private void HandleColliderHoldCollect(InteractionEvent interactionEvent)
        {
            // the first node was selected. the handle spawned and was collected in the next frame.
            if (_currentPath.Count != 1) return;

            var colliders = interactionEvent.Targets;

            EnchantmentPointerCollider handleCollider = null;
            foreach (var collider in colliders.Span)
                if (collider.TryGetComponent<EnchantmentPointerCollider>(out var enchantmentCollider)
                    && enchantmentCollider.PointerType == EnchantmentPointerColliderType.Handle)
                {
                    handleCollider = enchantmentCollider;
                    break;
                }

            if (handleCollider == null) return;

            _elementsHolder.TryGetEnchantmentHandle(out var handle);
            handle.Item2.HandleHold(true);

            _elementsHolder.TryFindEnchantmentNodeByIndex(
                index: _currentPath.Peek(),
                result: out var fromNode
            );

            _tempConnectionLines.Push(new TempNodeConnectionLine(
                                          startPosition: fromNode.Item2.transform.position,
                                          endPosition: handle.Item1.transform.position
                                      ));
        }

        private void HandleColliderDragCollect(InteractionEvent interactionEvent)
        {
            // a node is already selected. handle state is unknown.
            // if handle is held, and we collected a new legal node, we build a path between first node and this node.
            if (_currentPath.Count < 1) return;

            var colliders = interactionEvent.Targets;

            EnchantmentPointerCollider nodeCollider = null;
            foreach (var collider in colliders.Span)
                if (collider.TryGetComponent<EnchantmentPointerCollider>(out var enchantmentCollider)
                    && enchantmentCollider.PointerType == EnchantmentPointerColliderType.Node)
                {
                    nodeCollider = enchantmentCollider;
                    break;
                }

            if (nodeCollider == null) return;
            if (!_elementsHolder.TryGetEnchantmentHandle(out var handle)) return;
            if (!handle.Item2.IsHeld()) return;
            if (!TryGetNodeIndexByCollider(collider: nodeCollider, nodeIndex: out var nodeIndex)) return;

            var fromNodeId = _currentPath.Peek();
            var toNodeId   = nodeIndex;

            if (!IsConnectingLegal(fromNodeId: fromNodeId, toNodeId: toNodeId)) return;

            // fixate connection
            _currentPath.Push(nodeIndex);

            if (!_elementsHolder.TryFindEnchantmentNodeByIndex(index: toNodeId, result: out var toNode)) return;
            if (!_elementsHolder.TryFindEnchantmentNodeByIndex(index: fromNodeId, result: out var fromNode)) return;

            toNode.Item3.SetState(EnchantmentNodeViewState.ConnectedHeld);
            fromNode.Item3.SetState(EnchantmentNodeViewState.Completed);

            var currentTempLine = _tempConnectionLines.Peek();
            currentTempLine.UpdateEndPosition(newEndPosition: toNode.Item2.transform.position);

            _tempConnectionLines.Push(new TempNodeConnectionLine(
                                          startPosition: toNode.Item2.transform.position,
                                          endPosition: handle.Item1.transform.position
                                      ));
        }

        private void HandleColliderHoldOrDragEnd(InteractionEvent interactionEvent)
        {
            // the handle is spawned and held. since drag handles connections, no need to do it here.
            // but if there are no connections yet, and hold is released, we reset the path entirely.
            // releasing the handle should hide the handle.
            if (_currentPath.Count < 1) return;

            if (!_elementsHolder.TryGetEnchantmentHandle(out var handle)) return;
            if (!handle.Item2.IsHeld()) return;

            handle.Item2.HandleHold(false);

            var lastConnectedNodeId = _currentPath.Peek();
            if (_elementsHolder.TryFindEnchantmentNodeByIndex(index: lastConnectedNodeId, result: out var node))
            {
                if (node.Item3.GetState() == EnchantmentNodeViewState.UnconnectedHeld)
                    node.Item3.SetState(EnchantmentNodeViewState.UnconnectedIdle);
                else if (node.Item3.GetState() == EnchantmentNodeViewState.ConnectedHeld)
                    node.Item3.SetState(EnchantmentNodeViewState.ConnectedIdle);
            }

            if (_currentPath.Count == 1)
            {
                handle.Item2.Deactivate();
                _currentPath.Clear();
            }
            else
            {
                var lastNodeId = _currentPath.Peek();
                if (_elementsHolder.TryFindEnchantmentNodeByIndex(index: lastNodeId, result: out var lastNode))
                    handle.Item1.transform.position = lastNode.Item2.transform.position;
            }

            _tempConnectionLines.Pop();
        }

        private bool TryGetNodeIndexByCollider(EnchantmentPointerCollider collider, out int nodeIndex)
        {
            nodeIndex = -1;
            if (collider.PointerType != EnchantmentPointerColliderType.Node) return false;

            var nodePresenter = collider.GetComponentInParent<EnchantmentNodePresenter>();
            if (nodePresenter == null)
            {
                Debug.LogError("Node Presenter not found in parent of Node Collider.");
                return false;
            }

            if (_elementsHolder.TryFindEnchantmentNodeByPresenter(presenter: nodePresenter, result: out var node))
            {
                nodeIndex = node.Item1.Data.Index;
                return true;
            }

            Debug.LogError("Node Data not found for the given Node Presenter.");
            return false;
        }

        private bool IsConnectingLegal(int fromNodeId, int toNodeId)
        {
            for (var i = 0; i < _layout.Connections.Count; i++)
            {
                if ((_layout.Connections[i].NodeA.Index != fromNodeId ||
                     _layout.Connections[i].NodeB.Index != toNodeId)
                    &&
                    (_layout.Connections[i].NodeA.Index != toNodeId ||
                     _layout.Connections[i].NodeB.Index != fromNodeId)) continue;
                if (_currentConnections.Contains(_layout.Connections[i]))
                    return true;
            }

            return false;
        }

        internal class TempNodeConnectionLine
        {
            public readonly Vector2 StartPosition;
            public          Vector2 EndPosition;

            public TempNodeConnectionLine(Vector2 startPosition, Vector2 endPosition)
            {
                StartPosition = startPosition;
                EndPosition   = endPosition;
            }

            public void UpdateEndPosition(Vector2 newEndPosition)
            {
                EndPosition = newEndPosition;
            }
        }
    }
}
