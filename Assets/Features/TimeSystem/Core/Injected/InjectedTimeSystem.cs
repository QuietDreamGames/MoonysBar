using Features.TimeSystem.Interfaces;
using JetBrains.Annotations;
using VContainer;

namespace Features.TimeSystem.Core.Injected
{
    public class InjectedTimeSystem : TimeSystem
    {
        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public InjectedTimeSystem(IUpdateProvider updateProvider)
        {
            SetUpdateProvider(updateProvider);
        }
    }
}