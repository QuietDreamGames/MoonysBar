using Features.TimeSystem.Interfaces.Handlers;
using JetBrains.Annotations;

namespace Features.MixMinigame
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class MixGameLevelTimerHolder : IUpdateHandler
    {
        public float Timer { get; private set; }

        public void OnUpdate(float deltaTime)
        {
            Timer += deltaTime;
        }
    }
}