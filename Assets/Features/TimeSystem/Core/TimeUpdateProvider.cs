using System;
using Features.TimeSystem.Interfaces;
using UnityEngine;

namespace Features.TimeSystem.Core
{
    public class TimeUpdateProvider : MonoBehaviour, IUpdateProvider
    {
        public Action OnUpdate      { get; set; }
        public Action OnFixedUpdate { get; set; }
        public Action OnLateUpdate  { get; set; }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        private void OnDestroy()
        {
            OnUpdate      = null;
            OnFixedUpdate = null;
            OnLateUpdate  = null;
        }
    }
}