using Features.SceneLoader;
using UnityEngine;
using VContainer;

namespace Features.Boot
{
    public class BootStarter : MonoBehaviour
    {
        [Inject] private SceneLoaderService _sceneLoaderService;

        private void Start()
        {
            _sceneLoaderService.Load(1);
        }
    }
}