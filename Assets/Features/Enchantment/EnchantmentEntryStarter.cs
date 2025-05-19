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

            lineRendererObject.startWidth = 0.5f;
            lineRendererObject.endWidth   = 0.5f;

            lineRendererObject.positionCount = layout.Connections.Count;

            var path = FindLinePath(layout);

            for (var i = 0; i < path.Count; i++)
            {
                lineRendererObject.SetPosition(i,
                    _playingFieldService.ConvertRelativeToWorldPosition(layout.GetNodeByIndex(path[i]).InitialPosition));
            }

            // for (var i = 1; i < layout.Connections.Count; i++)
            // {
            //     var connection = layout.Connections[i];
            //     var startNode  = layout.Nodes[connection.NodeA.Index];
            //     var endNode    = layout.Nodes[connection.NodeB.Index];
            //
            //     lineRendererObject.SetPosition(i * 2,
            //         _playingFieldService.ConvertRelativeToWorldPosition(startNode.InitialPosition));
            //     lineRendererObject.SetPosition(i * 2 + 1,
            //         _playingFieldService.ConvertRelativeToWorldPosition(endNode.InitialPosition));
            // }
        }

        private List<Vector2> FindLinePath(EnchantmentGraphData graphData)
        {
            var edges = new List<GraphPathfinder.PathEdge>();

            for (var i = 0; i < graphData.Connections.Count; i++)
            {
                var connection = graphData.Connections[i];
                var startNode  = graphData.Nodes[connection.NodeA.Index];
                var endNode    = graphData.Nodes[connection.NodeB.Index];

                edges.Add(new GraphPathfinder.PathEdge(startNode.InitialPosition, endNode.InitialPosition));
            }

            graphData.Nodes.Sort((a, b) => a.Index.CompareTo(b.Index));
            var firstNode = graphData.Nodes[0];

            return GraphPathfinder.FindLongestPath(firstNode.Index, edges);
        }
    }

    public class GraphPathfinder
    {
        public static List<Vector2> FindLongestPath(Vector2 start, List<PathEdge> edges)
        {
            var longestPath = new List<Vector2>();

            void DFS(Vector2 current, List<Vector2> path, HashSet<PathEdge> visited)
            {
                path.Add(current);
                if (path.Count > longestPath.Count)
                    longestPath = new List<Vector2>(path);

                foreach (var edge in edges)
                    if (!visited.Contains(edge) && (edge.A == current || edge.B == current))
                    {
                        var next = edge.A == current ? edge.B : edge.A;
                        visited.Add(edge);
                        DFS(next, path, visited);
                        visited.Remove(edge);
                    }

                path.RemoveAt(path.Count - 1);
            }

            DFS(start, new List<Vector2>(), new HashSet<PathEdge>());

            return longestPath;
        }

        public class PathEdge
        {
            public Vector2 A, B;

            public PathEdge(Vector2 a, Vector2 b)
            {
                A = a;
                B = b;
            }

            // Undirected edge equality
            public bool Equals(PathEdge other)
            {
                return (A == other.A && B == other.B) || (A == other.B && B == other.A);
            }

            public override int GetHashCode()
            {
                return A.GetHashCode() ^ B.GetHashCode();
            }
        }
    }
}
