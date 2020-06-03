#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    public class AssetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        public static event Action<ScriptableObject> DeletingScriptableObject;
        public static event Action<GameObject> DeletingPrefab;

        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            if (asset != null) DeletingScriptableObject?.Invoke(asset);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab != null) DeletingPrefab?.Invoke(prefab);
            return AssetDeleteResult.DidNotDelete;
        }
    }
}

#endif