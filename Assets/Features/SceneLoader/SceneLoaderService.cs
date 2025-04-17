using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.SceneLoader
{
    public class SceneLoaderService
    {
        public Task Load(int sceneId, Action onLoaded = null)
        {
            return LoadScene(sceneId, onLoaded);
        }
        
        private async Task LoadScene(int sceneId, Action onLoaded = null)
        {
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(sceneId);

            while (!waitNextScene.isDone)
            {
                await Task.Yield();
            }

            onLoaded?.Invoke();
        }
    }
}