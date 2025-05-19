using Features.ObjectPools.BasePool;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.ObjectPools.InjectedPool
{
    public class InjectedPrefabPool : PrefabPool
    {
        private readonly IObjectResolver _objectResolver;

        public InjectedPrefabPool(IObjectResolver objectResolver,
            GameObject                            prefab,
            Transform                             root) : base(prefab, root)
        {
            _objectResolver = objectResolver;
        }

        protected override void CreateChildren(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var go = _objectResolver.Instantiate(Prefab, Parent);
                go.SetActive(false);
                Free.Add(go);
                Count++;
            }
        }
    }
}