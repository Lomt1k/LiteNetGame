using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scenes
{
    public class SceneLoader : MonoSingletone<SceneLoader>
    {
        public SceneType currentScene { get; private set; }
        public bool isLoading { get; private set; }

        private Coroutine _coroutine;

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
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            SceneManager.LoadScene(SceneType.LoadingScene.ToString()); //for unload previous scene
            _coroutine = StartCoroutine(LoadSceneCoroutine(sceneType, onLoading));
        }

        private IEnumerator LoadSceneCoroutine(SceneType sceneType, Action onLoading)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneType.ToString());
            onLoading?.Invoke();
            yield return new WaitUntil(() => asyncLoad.isDone);
            _coroutine = null;
            isLoading = false;
        }
        
        
        
    }
}

