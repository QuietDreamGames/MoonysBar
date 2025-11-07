using System.Collections.Generic;
using Features.CameraSystem;
using Features.Enchantment.Datas;
using Features.Enchantment.Presenters;
using Features.InputDispatching;
using UnityEngine;
using VContainer;

namespace Features.Enchantment
{
    public class EnchantmentPathController
    {
        [Inject] private readonly CameraHolderService                 _cameraHolderService;
        [Inject] private readonly EnchantmentElementsHolderAndUpdater _elementsHolder;

        private List<NodeConnection> _currentConnections;
        private Stack<int>           _currentPath;

        private EnchantmentGraphData _layout;

        private Stack<TempNodeConnectionLine> _tempConnectionLines;


        public void SetLayout(EnchantmentGraphData layout)
        {
            _layout = layout;

            _currentConnections  = new List<NodeConnection>(_layout.Connections);
            _currentPath         = new Stack<int>();
            _tempConnectionLines = new Stack<TempNodeConnectionLine>();

            if (_elementsHolder.TryGetEnchantmentHandle(out var handle))
                handle.Item2.Hide();
            else
                Debug.LogError("Enchantment Handle not found in Elements Holder.");
        }

        public void OnPointerColliderHeld(EnchantmentPointerCollider nodeCollider, bool withHandle, bool isHeld)
        {
            if (_layout == null) return;

            if (!TryGetNodeIndexByCollider(collider: nodeCollider, nodeIndex: out var nodeIndex))
                return;

            if (isHeld && _currentPath.Count == 0)
            {
                _currentPath.Push(nodeIndex);
                if (_elementsHolder.TryGetEnchantmentHandle(out var handle))
                {
                    handle.Item2.Show();
                    handle.Item1.transform.position = _cameraHolderService.MainCamera.ScreenToWorldPoint(
                        InputUtils.GetPrimaryPointerScreenPosition()
                    );
                    _tempConnectionLines.Push(new TempNodeConnectionLine(
                        startPosition: handle.Item1.transform.position,
                        endPosition:   handle.Item1.transform.position
                    ));
                }
            }

            if (isHeld && _currentPath.Count > 2 && nodeIndex == _currentPath.Peek())
            {
                _

            }
        }

        public void OnPointerColliderPassThrough()
        {
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
                if (!_currentConnections.Contains(_layout.Connections[i]))
                    return true;
            }

            return false;
        }

        private bool TryGetNodeIndexByCollider(EnchantmentPointerCollider collider, out int nodeIndex)
        {
            nodeIndex = -1;
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
    }

    internal class TempNodeConnectionLine
    {
        public Vector2 EndPosition;
        public Vector2 StartPosition;

        public TempNodeConnectionLine(Vector2 startPosition, Vector2 endPosition)
        {
            StartPosition = startPosition;
            EndPosition   = endPosition;

            DrawLine();
        }

        public void UpdateEndPosition(Vector2 newEndPosition)
        {
            EndPosition = newEndPosition;
            DrawLine();
        }

        private void DrawLine()
        {
            Debug.DrawLine(start: StartPosition, end: EndPosition, color: Color.green);
        }
    }
}
