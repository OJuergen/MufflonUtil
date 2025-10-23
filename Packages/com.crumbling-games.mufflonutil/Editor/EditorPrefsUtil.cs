using UnityEditor;
using UnityEngine;

namespace MufflonUtil.Editor
{
    public static class EditorPrefsUtil
    {
        public static bool Checkbox(string label, Object asset)
        {
            string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));
            var key = $"{guid}_{label}";
            bool currentValue = EditorPrefs.GetBool(key);
            bool newValue = EditorGUILayout.Toggle(label, currentValue);
            if (newValue != currentValue) EditorPrefs.SetBool(key, newValue);
            return newValue;
        }
    }
}