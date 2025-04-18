using System;
using Features.TimeSystem.Interfaces;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Features.TimeSystem.Core.Injected
{    
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class InjectedTimeUpdateProvider : IUpdateProvider, ITickable, IFixedTickable, ILateTickable
    {
        public Action OnUpdate      { get; set; }
        public Action OnFixedUpdate { get; set; }
        public Action OnLateUpdate  { get; set; }
        
        public void Tick()
        {
            OnUpdate?.Invoke();
        }

        public void FixedTick()
        {
            OnFixedUpdate?.Invoke();
        }

        public void LateTick()
        {
            OnLateUpdate?.Invoke();
        }
    }
}