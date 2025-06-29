using Prototype.Models;
using Prototype.UI;
using Prototype.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.Managers
{
    public sealed class SceneLoaderManager : Singleton<SceneLoaderManager>
    {
        [SerializeField] private List<SceneField> allSceneList;

        public AsyncOperation AsyncLoad { get; private set; }

        public static event Action<SceneName, SceneName> OnSceneTransitionStart;
        public static event Action OnSceneLoading;
        public static event Action<SceneName> OnSceneLoaded;

        private void Start()
        {
            if ((SceneName)SceneManager.GetActiveScene().buildIndex != SceneName.Office)
            {
                InitializeSceneTransition(SceneName.Office);
            }
        }

        public SceneField FindScenePerName(SceneName sceneName)
        {
            return allSceneList.Find(s => s.SceneName == sceneName);
        }

        public void InitializeSceneTransition(SceneName sceneName)
        {
            GameManager.GameState.SetState(GameStatus.LoadingNewScene);
            ExecuteSceneTransition(sceneName);
            GameManager.GameState.SetState(GameStatus.None);
        }

        private void ExecuteSceneTransition(SceneName sceneName)
        {
            StartCoroutine(PerfomLoadSceneAsync(FindScenePerName(sceneName)));
        }

        private IEnumerator PerfomLoadSceneAsync(SceneField nextScene)
        {
            OnSceneTransitionStart?.Invoke((SceneName)SceneManager.GetActiveScene().buildIndex, nextScene.SceneName);

            yield return StartCoroutine(SceneTransitionUIAnimation.SceneTransitionUI.ZoomInCoroutine());
            yield return StartCoroutine(LoadSceneAsyncCoroutine(nextScene));
            yield return StartCoroutine(SceneTransitionUIAnimation.SceneTransitionUI.ZoomOutCoroutine());
        }

        private IEnumerator LoadSceneAsyncCoroutine(SceneField sceneField)
        {
            AsyncLoad = SceneManager.LoadSceneAsync(sceneField);

            OnSceneLoading?.Invoke();

            while (!AsyncLoad.isDone)
            {
                yield return null;
            }

            OnSceneLoaded?.Invoke(sceneField.SceneName);

            yield return new WaitForSeconds(1f); // Tempo para carregar tudo antes da cena for exposta
        }
    }
}
