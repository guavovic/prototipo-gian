using UnityEngine;

namespace Prototype.Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var objects = FindObjectsOfType(typeof(T)) as T[];

                if (objects?.Length > 0)
                {
                    _instance = objects[0];
                }

                if (objects?.Length > 1)
                {
                    Debug.LogError($"[Singleton] There is more than one {typeof(T).Name} in the scene.");
                }

                if (_instance != null)
                {
                    return _instance;
                }

#if UNITY_EDITOR
                Debug.LogWarning($"[Singleton] There is no instance of {typeof(T).Name} in the scene. Creating one now");
#endif
                GameObject obj = new GameObject();
                obj.name = $"_{typeof(T).Name}";
                _instance = obj.AddComponent<T>();

                return _instance;
            }
        }

        [SerializeField] private bool dontDestroyOnLoad;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }

            if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}