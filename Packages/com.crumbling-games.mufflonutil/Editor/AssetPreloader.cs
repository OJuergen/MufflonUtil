using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MufflonUtil.Editor
{
    [InitializeOnLoad]
    public static class AssetPreloader
    {
        static AssetPreloader()
        {
            ScriptableObject[] assets = AssetDatabase.FindAssets("t:ScriptableObject")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>)
                .Where(so => so is IPreloadedAsset)
                .ToArray();

            HashSet<Object> preloadedAssets = PlayerSettings.GetPreloadedAssets().MakeHashSet();
            foreach (ScriptableObject asset in assets) preloadedAssets.Add(asset);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }
    }
}