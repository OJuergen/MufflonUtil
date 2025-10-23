#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    public class AssetPostProcessor : AssetPostprocessor
    {
        public static event Action<GameObject> ImportedPrefab;
        public static event Action<ScriptableObject> ImportedScriptableObject;
        public static event Action<string> DeletedAsset;

        private static void OnPostprocessAllAssets(string[] importedAssets,
                                                   string[] deletedAssets,
                                                   string[] movedAssets,
                                                   string[] movedFromAssetPaths)
        {
            foreach (string importedAsset in importedAssets)
            {
                var scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(importedAsset);
                if (scriptableObject != null) ImportedScriptableObject?.Invoke(scriptableObject);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(importedAsset);
                if (prefab != null) ImportedPrefab?.Invoke(prefab);
            }
            foreach (string deletedAsset in deletedAssets)
            {
                DeletedAsset?.Invoke(deletedAsset);
            }
        }
    }
}

#endif