using UnityEngine;

public enum GameStatus
{
    Playing,
    Paused,
    InMenu,
    LoadingNewScene
}

public static class GameState
{
    public static GameStatus CurrentStatus { get; private set; } = GameStatus.Playing;

    public static void SetStatus(GameStatus status)
    {
        CurrentStatus = status;
#if UNITY_EDITOR
        Debug.Log($"Game state changed to: {CurrentStatus}");
#endif
    }

    public static bool IsPaused => CurrentStatus == GameStatus.Paused;
}
