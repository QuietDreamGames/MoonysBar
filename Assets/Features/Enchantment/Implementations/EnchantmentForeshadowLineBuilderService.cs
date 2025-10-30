using System.Collections.Generic;
using Features.Enchantment.Datas;
using Features.Enchantment.Interfaces;
using UnityEngine;
using VContainer;

namespace Features.Enchantment.Implementations
{
    public class EnchantmentForeshadowLineBuilderService : IEnchantmentForeshadowLineBuilderService
    {
        private readonly List<GameObject>                _lineParts = new();
        [Inject] private ILineForeshadowElementsFabric   _lineForeshadowElementsFabric;
        [Inject] private IEnchantmentPlayingFieldService _playingFieldService;

        public void BuildForeshadowLine(EnchantmentGraphData layout)
        {
            _lineParts.AddRange(_lineForeshadowElementsFabric.CreateLineParts(layout.Connections.Count));

            for (var i = 0; i < layout.Connections.Count; i++)
            {
                var connection = layout.Connections[i];
                var linePart   = _lineParts[i];

                var startPosition = connection.NodeA.Position;
                var endPosition   = connection.NodeB.Position;

                var direction = _playingFieldService.ConvertRelativeToWorldPosition(endPosition - startPosition);
                var distance  = direction.magnitude;
                direction.Normalize();

                linePart.transform.position = _playingFieldService.ConvertRelativeToWorldPosition(startPosition);

                linePart.transform.rotation = Quaternion.FromToRotation(
                    fromDirection: Vector3.right,
                    toDirection: direction
                );

                var spriteRenderer = linePart.GetComponent<SpriteRenderer>();

                if (spriteRenderer == null)
                    throw new MissingComponentException(
                        $"Missing SpriteRenderer component on line part GameObject '{linePart.name}'"
                    );

                var size = spriteRenderer.size;
                size.x              = distance;
                spriteRenderer.size = size;
            }
        }
    }
}
