using Features.InputDispatching;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.Scopes
{
    public class TestInputScope : LifetimeScope
    {
        [SerializeField] private InputPointerDebugger inputPointerDebugger;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(inputPointerDebugger);
        }
    }
}
