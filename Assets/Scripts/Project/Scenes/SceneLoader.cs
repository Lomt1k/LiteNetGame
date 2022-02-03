using System;
using Project.UI.Windows.LoadingScreenWindow;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scenes
{
    public class SceneLoader : MonoSingletone<SceneLoader>
    {
        public SceneType currentScene { get; private set; }
        public bool isLoading { get; private set; }

        private LoadingScreenWindow _loadingWindow;

        private void Awake()
        {
            SetupCurrentScene();
        }

        private void SetupCurrentScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (Enum.TryParse<SceneType>(currentSceneName, out var activeScene))
            {
                currentScene = activeScene;
            }
        }

        public void LoadScene(SceneType sceneType, Action onLoading = null)
        {
            if (sceneType == SceneType.LoadingScene)
            {
                Debug.LogError($"Can not load {SceneType.LoadingScene}");
                return;
            }

            isLoading = true;
            _loadingWindow = UI.Windows.WindowsManager.CreateWindow<LoadingScreenWindow>();
            SceneManager.LoadScene(SceneType.LoadingScene.ToString()); //for unload previous scene
            onLoading?.Invoke();
            
            var asyncLoad = SceneManager.LoadSceneAsync(sceneType.ToString());
            asyncLoad.completed += operation =>
            {
                isLoading = false;
                _loadingWindow.Close();
            };
        }
        
        
        
    }
}

