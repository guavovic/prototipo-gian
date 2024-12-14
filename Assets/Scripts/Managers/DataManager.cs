using UnityEngine;
using System.Collections.Generic;
using Prototype.Utils;
using System.Linq;

namespace Prototype.Data
{
    public sealed class DataManager : Singleton<DataManager>
    {
        private readonly Dictionary<string, List<ScriptableObject>> _dataDictionary = new Dictionary<string, List<ScriptableObject>>();

        protected override void Awake()
        {
            base.Awake();
            LoadAllData();
        }

        private void LoadAllData()
        {
            ScriptableObject[] data = Resources.LoadAll<ScriptableObject>("Data");

            foreach (var item in data)
            {
                if (_dataDictionary.ContainsKey(item.name))
                {
                    _dataDictionary[item.name].Add(item);
                }
                else
                {
                    _dataDictionary[item.name] = new List<ScriptableObject> { item };
                }
            }

            foreach (var item in _dataDictionary)
            {
                Debug.Log(item);
            }
        }

        public List<T> GetData<T>() where T : ScriptableObject
        {
            string key = typeof(T).Name;

            if (_dataDictionary.ContainsKey(key))
            {
                return _dataDictionary[key].OfType<T>().ToList();
            }

            return null;
        }
    }
}