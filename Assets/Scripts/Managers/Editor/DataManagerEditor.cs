using UnityEditor;
using UnityEngine;

namespace Prototype.Data
{
    [CustomEditor(typeof(DataManager))]
    public class DataManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DataManager dataManager = (DataManager)target;

            //foreach (var entry in dataManager.GetData<ScriptableObject>())
            //{
            //    EditorGUILayout.LabelField(entry.name);

            //    foreach (var item in entry.name)
            //    {
            //      //  EditorGUILayout.ObjectField(item, typeof(ScriptableObject), false);
            //    }
            //}
        }
    }
}
