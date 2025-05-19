using System.Collections.Generic;
using Features.TimeSystem.Interfaces;
using Features.TimeSystem.Interfaces.Handlers;
using Features.TimeSystem.Interfaces.Injected;
using JetBrains.Annotations;

namespace Features.TimeSystem.Core.Injected
{
    public class InjectedTimeCollector : ITransientTimeCollector
    {
        public List<IUpdateHandler>      UpdateHandlers      { get; }
        public List<IFixedUpdateHandler> FixedUpdateHandlers { get; }
        public List<ILateUpdateHandler>  LateUpdateHandlers  { get; }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public InjectedTimeCollector()
        {
            UpdateHandlers      = new List<IUpdateHandler>();
            FixedUpdateHandlers = new List<IFixedUpdateHandler>();
            LateUpdateHandlers  = new List<ILateUpdateHandler>();
        }
    }
}