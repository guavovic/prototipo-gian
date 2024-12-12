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

        public SceneField CurrentScene { get; private set; }
        public AsyncOperation AsyncLoad { get; private set; }

        public event Action OnSceneTransitionStart;
        public event Action OnSceneLoading;
        public event Action OnSceneLoaded;

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
            OnSceneTransitionStart?.Invoke();

            yield return StartCoroutine(SceneTransitionUIAnimation.SceneTransitionUI.ZoomInCoroutine());
            yield return StartCoroutine(LoadSceneAsyncCoroutine(nextScene));
            yield return StartCoroutine(SceneTransitionUIAnimation.SceneTransitionUI.ZoomOutCoroutine());
        }

        private IEnumerator LoadSceneAsyncCoroutine(SceneField sceneField)
        {
            AsyncLoad = SceneManager.LoadSceneAsync(sceneField);
            CurrentScene = sceneField;

            OnSceneLoading?.Invoke();

            while (!AsyncLoad.isDone)
            {
                yield return null;
            }

            OnSceneLoaded?.Invoke();

            yield return new WaitForSeconds(1f); // Tempo para carregar tudo antes da cena for exposta
        }
    }
}