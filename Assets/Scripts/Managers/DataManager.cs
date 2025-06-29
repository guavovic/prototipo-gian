using UnityEngine;
using System.Collections.Generic;
using Prototype.Utils;
using System.Linq;
using System;

namespace Prototype.Data
{
    public sealed class DataManager : Singleton<DataManager>
    {
        private const string _DATA_DIRECTORY = "Data";
        private  readonly Dictionary<Type, List<ScriptableObject>> _dataDictionary = new Dictionary<Type, List<ScriptableObject>>();

        protected override void Awake()
        {
            base.Awake();
            LoadAllData();
        }

        private void LoadAllData()
        {
            ScriptableObject[] data = Resources.LoadAll<ScriptableObject>(_DATA_DIRECTORY);

            foreach (var item in data)
            {
                Type itemType = item.GetType();

                if (_dataDictionary.ContainsKey(itemType))
                {
                    _dataDictionary[itemType].Add(item);
                }
                else
                {
                    _dataDictionary[itemType] = new List<ScriptableObject> { item };
                }
            }
        }

        public List<T> GetData<T>() where T : ScriptableObject
        {
            Type key = typeof(T);

            if (_dataDictionary.ContainsKey(key))
            {
                return _dataDictionary[key].OfType<T>().ToList();
            }

            return null;
        }
    }
}