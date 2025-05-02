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
        
        void ITickable.Tick()
        {
            OnUpdate?.Invoke();
        }

        void IFixedTickable.FixedTick()
        {
            OnFixedUpdate?.Invoke();
        }

        void ILateTickable.LateTick()
        {
            OnLateUpdate?.Invoke();
        }
    }
}