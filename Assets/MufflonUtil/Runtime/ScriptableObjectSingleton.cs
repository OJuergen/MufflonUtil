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
                        $"Failed to locate ScriptableObject Singleton of type {typeof(T)}. " +
                        "Please make sure you create and load a respective asset. " +
                        "Be aware that asset loading in the editor and the build differ. " +
                        "To load the asset, you can put it in a Resources folder. " +
                        "Note, however, that this affects the loading behaviour of the app. " +
                        "Consider referencing it from a script on a scene object (see MufflonUtil.AssetLoader).");
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