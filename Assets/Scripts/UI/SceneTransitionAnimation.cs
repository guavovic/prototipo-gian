using UnityEngine;

public sealed class SceneTransitionAnimation : MonoBehaviour
{
    public SceneTransitionUI SceneTransitionUI { get; private set; }

    public static SceneTransitionAnimation Instance { get; private set; }

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

        SceneTransitionUI = GetComponentInChildren<SceneTransitionUI>();
    }
}