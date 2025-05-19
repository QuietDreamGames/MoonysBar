using VContainer;
using VContainer.Unity;

namespace Features.MixMinigame
{
    public class MixGameEntryPoint : IStartable
    {
        [Inject] private readonly MixGamePointerCollisionService _mixGamePointerCollisionService;

        public void Start()
        {
        }
    }
}
