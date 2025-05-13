using System;
using UnityEngine;

namespace Features.Helpers
{
    [Serializable]
    public class InterfaceWrapper<T> where T : class
    {
        [SerializeField] private MonoBehaviour component;

        public T Value => component as T;
    }
}
