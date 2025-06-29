using Prototype.Models;
using Prototype.Utils;
using System;

namespace Prototype.Managers
{
    public sealed class GameManager : Singleton<GameManager>
    {
        public static Order CurrentOrder { get; private set; }
        public static void SetCurrentOrderSelected(Order order) => CurrentOrder = order;
        public SceneName CurrentScene { get; private set; }
        public static GameState GameState { get; private set; } = new GameState();

        public static event Action OnOfficeSceneStarted;
        public static event Action OnParkingSceneStarted;

        private void Start()
        {
            // temporario
            SceneLoaded(SceneName.Office);
        }

        private void OnEnable()
        {
            SceneLoaderManager.OnSceneLoaded += SceneLoaded;
        }

        private void OnDisable()
        {
            SceneLoaderManager.OnSceneLoaded -= SceneLoaded;
        }

        public static void SwitchScene(SceneName sceneName)
        {
            SceneLoaderManager.Instance.InitializeSceneTransition(sceneName);
        }

        private void SceneLoaded(SceneName scene)
        {
            CurrentScene = scene;
            GameState.SetState(GameStatus.Playing);

            switch (CurrentScene)
            {
                case SceneName.Office:
                    OnOfficeSceneStarted?.Invoke();
                    break;

                case SceneName.Parking:
                    OnParkingSceneStarted?.Invoke();
                    break;
            }
        }
    }
}