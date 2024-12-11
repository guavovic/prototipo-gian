using UnityEngine;

public enum GameStatus
{
    None,
    Playing,
    Paused,
    InMenu,
    LoadingNewScene
}

public static class GameState
{
    public static GameStatus CurrentStatus { get; private set; } = GameStatus.None;

    public static void SetState(GameStatus status)
    {
        CurrentStatus = status;
#if UNITY_EDITOR
        Debug.Log($"Game state changed to: {CurrentStatus}");
#endif
    }

    public static bool IsPaused => CurrentStatus == GameStatus.Paused;
}
