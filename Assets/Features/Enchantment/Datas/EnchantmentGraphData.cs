using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Features.Enchantment.Datas
{
    public class EnchantmentGraphData
    {
        public EnchantmentGraphData(string rawLayoutText)
        {
            (Nodes, Connections) = EnchantmentGraphParser.Parse(rawLayoutText);
        }

        public List<EnchantmentNodeData> Nodes       { get; }
        public List<NodeConnection>      Connections { get; }

        public EnchantmentNodeData GetNodeByIndex(int index)
        {
            return Nodes.FirstOrDefault(node => node.Index == index);
        }
    }

    public class NodeConnection
    {
        public NodeConnection(EnchantmentNodeData nodeA, EnchantmentNodeData nodeB)
        {
            NodeA = nodeA;
            NodeB = nodeB;
        }

        public EnchantmentNodeData NodeA { get; }
        public EnchantmentNodeData NodeB { get; }
    }

    internal static class EnchantmentGraphParser
    {
        public static (List<EnchantmentNodeData>, List<NodeConnection>) Parse(string rawLayoutText)
        {
            rawLayoutText = rawLayoutText.Replace("\r", string.Empty);
            var lines       = rawLayoutText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var cultureInfo = new CultureInfo("en-US");

            var nodes           = new List<EnchantmentNodeData>();
            var rawConnections  = new List<Vector2Int>();
            var nodeConnections = new List<NodeConnection>();

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;
                if (parts.First().First() == '#') continue; // Skip comments

                if (parts.Length == 3)
                {
                    if (!int.TryParse(parts[0], out var index)) continue;
                    if (!float.TryParse(parts[1], NumberStyles.Number, cultureInfo, out var positionX)) continue;
                    if (!float.TryParse(parts[2], NumberStyles.Number, cultureInfo, out var positionY)) continue;

                    nodes.Add(new EnchantmentNodeData(index, new Vector2(positionX, positionY)));
                    continue;
                }

                if (parts.Length != 2) continue;

                if (!int.TryParse(parts[0], out var indexA)) continue;
                if (!int.TryParse(parts[1], out var indexB)) continue;

                rawConnections.Add(new Vector2Int(indexA, indexB));
            }

            foreach (var rawConnection in rawConnections)
            {
                var nodeConnection = FindConnectionInVector(nodes, rawConnection);
                nodeConnections.Add(nodeConnection);
            }

            return (nodes, nodeConnections);
        }

        private static NodeConnection FindConnectionInVector(List<EnchantmentNodeData> nodes, Vector2Int vector)
        {
            EnchantmentNodeData nodeA = null;
            EnchantmentNodeData nodeB = null;

            for (var i = 0; i < nodes.Count; i++)
            {
                if (vector.x == nodes[i].Index && nodeA == null)
                    nodeA = nodes[i];
                if (vector.y == nodes[i].Index && nodeB == null)
                    nodeB = nodes[i];
                if (nodeA != null && nodeB != null) return new NodeConnection(nodeA, nodeB);
            }

            throw new Exception($"Can't find nodes with specified indexes: {vector.x} and {vector.y}");
        }
    }
}
