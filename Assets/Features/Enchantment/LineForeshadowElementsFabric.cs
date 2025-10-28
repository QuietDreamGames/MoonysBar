using System.Collections.Generic;
using UnityEngine;

namespace Features.Enchantment
{
    public class LineForeshadowElementsFabric : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer linePartPrefab;
        [SerializeField] private SpriteRenderer lineEndPrefab;
        [SerializeField] private SpriteRenderer line90DegreeTurnPrefab;

        public List<GameObject> CreateLineParts(int count)
        {
            var parts = new List<GameObject>();
            for (var i = 0; i < count; i++)
            {
                var part = Instantiate(linePartPrefab, transform);
                parts.Add(part.gameObject);
            }

            return parts;
        }
    }
}
