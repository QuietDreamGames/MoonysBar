using System.Collections.Generic;
using Features.Enchantment.Datas;
using UnityEngine;
using VContainer;

namespace Features.Enchantment
{
    public class EnchantmentEntryStarter : MonoBehaviour
    {
        [SerializeField] private EnchantmentNodesLayoutScriptableObject layoutScriptableObject;
        [SerializeField] private LineRenderer                           lineRendererObject;

        [Inject] private EnchantmentPlayingFieldService _playingFieldService;

        private void Awake()
        {
            var layout = layoutScriptableObject.GetLayout();
            if (layout == null)
            {
                Debug.LogError("Layout is null.");
                return;
            }

            lineRendererObject.startWidth = 1f;
            lineRendererObject.endWidth   = 1f;

            lineRendererObject.numCornerVertices = 50;
            lineRendererObject.numCapVertices    = 50;

            lineRendererObject.positionCount = layout.Connections.Count;

            var path = FindLinePath(layout);

            for (var i = 0; i < path.Count; i++)
                lineRendererObject.SetPosition(i,
                    _playingFieldService.ConvertRelativeToWorldPosition(layout.GetNodeByIndex(path[i])
                        .InitialPosition));
        }

        private static List<int> FindLinePath(EnchantmentGraphData graphData)
        {
            var edges = new List<(int, int)>();

            for (var i = 0; i < graphData.Connections.Count; i++)
            {
                var connection = graphData.Connections[i];
                var startNode  = graphData.Nodes[connection.NodeA.Index];
                var endNode    = graphData.Nodes[connection.NodeB.Index];

                edges.Add((startNode.Index, endNode.Index));
            }

            // let's suppose edges are built in the way they form the path we need.
            // then we only need to use the first edge's start node as the start node,
            // and all the next edges finish nodes as the path

            var path = new List<int>();
            path.Add(graphData.Connections[0].NodeA.Index);
            for (var i = 0; i < edges.Count; i++)
            {
                var connection = edges[i];
                path.Add(connection.Item2);
            }

            return path;
            // return GraphHelper.FindLongestPath(firstNode.Index, edges);
        }
    }
}
