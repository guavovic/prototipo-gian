using UnityEngine;

namespace Prototype.Models
{
    public class GameState
    {
        public GameStatus CurrentStatus { get; private set; } = GameStatus.None;

        public void SetState(GameStatus status)
        {
            CurrentStatus = status;
#if UNITY_EDITOR
            Debug.Log($"Game state changed to: {CurrentStatus}");
#endif
        }

        public bool IsPaused => CurrentStatus != GameStatus.Playing;
    }
}