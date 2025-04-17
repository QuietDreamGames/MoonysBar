using System;

namespace Features.TimeSystem.Interfaces
{
    public interface IUpdateProvider
    {
        Action OnUpdate      { get; set; }
        Action OnFixedUpdate { get; set; }
        Action OnLateUpdate  { get; set; }
    }
}