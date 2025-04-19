using Features.ObjectPools.BasePool;
using UnityEngine;
using VContainer;

namespace Features.ObjectPools.InjectedPool
{
    public class InjectedGameObjectPool<T> : GameObjectPool<T> where T : MonoBehaviour
    {
        private readonly IObjectResolver _objectResolver;
        
        public InjectedGameObjectPool(IObjectResolver objectResolver, Transform root) : base(root)
        {
            _objectResolver = objectResolver;
        }
        
        protected override PrefabPool GetPool(GameObject prefab)
        {
            if (!Pools.ContainsKey(prefab))
            {
                Pools[prefab] = new InjectedPrefabPool(_objectResolver, prefab, Parent);
            }

            return Pools[prefab];
        }
    }
}