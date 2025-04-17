using System.Collections.Generic;
using Features.TimeSystem.Interfaces.Handlers;

namespace Features.TimeSystem.Interfaces
{
    public interface ITimeCollector
    {
        List<IUpdateHandler>      UpdateHandlers      { get; }
        List<IFixedUpdateHandler> FixedUpdateHandlers { get; }
        List<ILateUpdateHandler>  LateUpdateHandlers  { get; }
    }
}