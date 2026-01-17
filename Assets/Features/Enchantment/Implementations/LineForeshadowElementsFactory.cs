using System.Collections.Generic;
using Features.Enchantment.Interfaces;
using UnityEngine;

namespace Features.Enchantment.Implementations
{
    public class LineForeshadowElementsFactory : MonoBehaviour, ILineForeshadowElementsFactory
    {
        [SerializeField] private SpriteRenderer linePartPrefab;
        [SerializeField] private SpriteRenderer lineEndPrefab;

        public GameObject CreateLineEnd()
        {
            var end = Instantiate(lineEndPrefab, transform);
            return end.gameObject;
        }

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
