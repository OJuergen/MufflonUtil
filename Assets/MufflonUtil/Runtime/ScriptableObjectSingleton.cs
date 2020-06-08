using System.Linq;
using UnityEngine;

namespace MufflonUtil
{
    public class ScriptableObjectSingleton : ScriptableObject, ISingleton
    { }

    public class ScriptableObjectSingleton<T> : ScriptableObjectSingleton where T : ScriptableObjectSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                if (_instance == null)
                    Debug.LogWarning(
                        "Failed to locate ScriptableObject Singleton. " +
                        "Please make sure you create and load a respective asset. " +
                        "Unity loads assets that are referenced by scene objects. " +
                        "Consider adding a MufflonUtil.AssetLoader script to an object in your scene.");
                return _instance;
            }
        }

        protected void OnEnable()
        {
#if UNITY_EDITOR
            AssetPostProcessor.ImportedScriptableObject += OnScriptableObjectImported;
#endif
        }

        protected void OnDisable()
        {
#if UNITY_EDITOR
            AssetPostProcessor.ImportedScriptableObject -= OnScriptableObjectImported;
#endif
        }

#if UNITY_EDITOR
        private void OnScriptableObjectImported(ScriptableObject obj)
        {
            if (UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}").Length > 1)
            {
                Debug.LogError($"Created an asset of singleton type {typeof(T)} multiple times. "
                               + $"Please make sure there is only one asset of type {typeof(T)}");
                Resources.UnloadAsset(_instance);
                _instance = (T) this;
            }
        }
#endif
    }
}