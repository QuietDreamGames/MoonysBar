using System.Collections.Generic;
using UnityEngine;

namespace Features.ObjectPools.BasePool
{
    public class PrefabPool
    {
        protected readonly GameObject Prefab;

        protected readonly List<GameObject> Used;
        protected readonly List<GameObject> Free;

        protected int Count;

        protected readonly Transform Parent;

        public PrefabPool(GameObject prefab, Transform root)
        {
            Prefab = prefab;

            var go = new GameObject($"{prefab.name}");
            go.SetActive(false);

            Parent = go.transform;
            Parent.SetParent(root, false);
            Parent.position = Vector3.zero;

            Used = new List<GameObject>(10);
            Free = new List<GameObject>(10);

            Count = 0;
        }

        public void Prewarm(int newCount)
        {
            var diff = newCount - Count;

            if (diff <= 0)
                return;

            CreateChildren(diff);
        }

        public GameObject Spawn()
        {
            if (Free.Count == 0)
                CreateChildren(1);

            var go = Free[0];
            Used.Add(go);
            Free.Remove(go);

            return go;
        }

        public void Despawn(GameObject go)
        {
            if (!Used.Contains(go))
            {
                Debug.LogError($"{go.name} is not in pool!");
                return;
            }

            if (Free.Contains(go))
            {
                Debug.LogError($"{go.name} is already in pool!");
                return;
            }

            go.transform.SetParent(Parent, false);
            go.SetActive(false);

            Free.Add(go);
            Used.Remove(go);
        }

        protected virtual void CreateChildren(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var go = Object.Instantiate(Prefab, Parent);
                go.SetActive(false);
                Free.Add(go);
                Count++;
            }
        }
    }
}