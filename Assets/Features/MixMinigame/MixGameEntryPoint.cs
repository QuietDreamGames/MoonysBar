using Features.InputDispatching;
using VContainer;
using VContainer.Unity;

namespace Features.MixMinigame
{
    public class MixGameEntryPoint : IStartable
    {
        [Inject]
        private readonly InputPointerGameObjectsCollisionService _inputPointerGameObjectsCollisionService;
        
        public void Start()
        {
        }
    }
}