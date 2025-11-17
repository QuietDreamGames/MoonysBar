using System.Collections.Generic;
using Features.TimeSystem.Interfaces.Handlers;
using Features.TimeSystem.Interfaces.Injected;

namespace Features.InputDispatching
{
    public class InputTimeCollector : ITransientTimeCollector
    {
        public List<IUpdateHandler>      UpdateHandlers      { get; }
        public List<IFixedUpdateHandler> FixedUpdateHandlers { get; }
        public List<ILateUpdateHandler>  LateUpdateHandlers  { get; }
    }
}
