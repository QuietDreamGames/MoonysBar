using System.Collections.Generic;
using UnityEngine;

namespace Features.ObjectPools.BasePool
{
    public class GameObjectPool<T> where T : MonoBehaviour
    {
        protected readonly Dictionary<GameObject, PrefabPool> Pools;
        
        protected readonly Transform Parent;
        
        public GameObjectPool(Transform root)
        {
            Pools = new Dictionary<GameObject, PrefabPool>();
            
            var go = new GameObject($"ObjectPool::{typeof(T).Name}");
            
            Parent = go.transform;
            Parent.SetParent(root, false);
        }
        
        public void Prewarm(GameObject prefab, int newCount)
        {
            var pool = GetPool(prefab);
            pool.Prewarm(newCount);
        }
        
        public T Spawn(GameObject prefab, Transform newParent)
        {
            var pool = GetPool(prefab);
            var go   = pool.Spawn();
            go.transform.SetParent(newParent, false);

            go.gameObject.SetActive(true);
            var component = go.GetComponent<T>();
            return component;
        }
        
        public void Despawn(GameObject prefab, T component)
        {
            var componentTransform = component.transform;
            componentTransform.SetParent(Parent, false);
            componentTransform.position = Parent.position;

            if (prefab == null)
            {
                Debug.LogError($"{prefab.name} is not in pool!");
                return;
            }
            
            var pool = GetPool(prefab);
            pool.Despawn(component.gameObject);
        }
        
        protected virtual PrefabPool GetPool(GameObject prefab)
        {
            if (!Pools.ContainsKey(prefab))
            {
                Pools[prefab] = new PrefabPool(prefab, Parent);
            }

            return Pools[prefab];
        }
    }
}