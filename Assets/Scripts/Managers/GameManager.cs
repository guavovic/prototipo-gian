using Prototype.Models;
using Prototype.Utils;
using System;

namespace Prototype.Managers
{
    public sealed class GameManager : Singleton<GameManager>
    {
        public const int MIN_VEHICLE_COUNT = 2;
        public const int MAX_VEHICLE_COUNT = 6;
        public const int MAXIMUM_ACTIVE_ORDERS_SIMULTANEOUSLY = 3;

        public static Order CurrentOrder { get; private set; }
        public static void SetCurrentOrderSelected(Order order) => CurrentOrder = order;

        public static GameState GameState { get; private set; } = new GameState();

        public static event Action OnGameStarted;

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            GameState.SetState(GameStatus.Playing);
            OnGameStarted?.Invoke();
        }

        public static void SwitchScene(SceneName sceneName)
        {
            SceneLoaderManager.Instance.InitializeSceneTransition(sceneName);
        }
    }
}