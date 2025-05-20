using System.Collections.Generic;

namespace Features.Helpers
{
    public static class GraphHelper
    {
        public static List<int> FindLongestPath(int startNode, List<(int, int)> edges)
        {
            // Build adjacency list with edge index to prevent reuse
            var graph = new Dictionary<int, List<(int neighbor, int edgeIndex)>>();
            for (var i = 0; i < edges.Count; i++)
            {
                var (u, v) = edges[i];
                if (!graph.ContainsKey(u)) graph[u] = new List<(int neighbor, int edgeIndex)>();
                if (!graph.ContainsKey(v)) graph[v] = new List<(int neighbor, int edgeIndex)>();
                graph[u].Add((v, i));
                graph[v].Add((u, i)); // undirected
            }

            var       visitedEdges = new bool[edges.Count];
            var       currentPath  = new List<int>();
            List<int> longestPath  = new();

            Dfs(startNode);
            return longestPath;

            void Dfs(int node)
            {
                currentPath.Add(node);

                if (currentPath.Count > longestPath.Count)
                    longestPath = new List<int>(currentPath);

                foreach (var (neighbor, edgeIndex) in graph[node])
                {
                    if (visitedEdges[edgeIndex]) continue;

                    visitedEdges[edgeIndex] = true;
                    Dfs(neighbor);
                    visitedEdges[edgeIndex] = false;
                }

                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }
    }
}
