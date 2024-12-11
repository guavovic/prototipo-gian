using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoaderManager : MonoBehaviour
{
    [SerializeField] private List<SceneField> allSceneList;

    public SceneField CurrentScene { get; private set; }
    public AsyncOperation AsyncLoad { get; private set; }

    public static SceneLoaderManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public SceneField FindScenePerName(SceneName sceneName)
    {
        return allSceneList.Find(s => s.SceneName == sceneName);
    }

    public void InitializeSceneTransition(SceneName sceneName)
    {
        GameState.SetState(GameStatus.LoadingNewScene);
        ExecuteSceneTransition(sceneName);
        GameState.SetState(GameStatus.None);
    }

    private void ExecuteSceneTransition(SceneName sceneName)
    {
        StartCoroutine(PerfomLoadSceneAsync(FindScenePerName(sceneName)));
    }

    private IEnumerator PerfomLoadSceneAsync(SceneField nextScene)
    {
        yield return StartCoroutine(SceneTransitionAnimation.Instance.SceneTransitionUI.ZoomInCoroutine());
        yield return StartCoroutine(LoadSceneAsyncCoroutine(nextScene));
        CurrentScene = nextScene;
        yield return StartCoroutine(SceneTransitionAnimation.Instance.SceneTransitionUI.ZoomOutCoroutine());
    }

    private IEnumerator LoadSceneAsyncCoroutine(SceneField sceneField)
    {
        AsyncLoad = SceneManager.LoadSceneAsync(sceneField);

        AsyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);

        AsyncLoad.allowSceneActivation = true;

        //wait until the asynchronous scene fully loads
        while (!AsyncLoad.isDone)
            yield return null;
    }
}