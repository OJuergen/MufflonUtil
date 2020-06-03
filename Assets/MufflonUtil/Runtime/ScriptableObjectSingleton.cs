using System.Linq;
using UnityEngine;

namespace MufflonUtil
{
    public class ScriptableObjectSingleton : ScriptableObject, ISingleton
    { }

    public class ScriptableObjectSingleton<T> : ScriptableObjectSingleton where T : ScriptableObjectSingleton<T>
    {
        private static T _instance;
        public static T Instance =>
            _instance = _instance ? _instance : Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

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

        private static void OnScriptableObjectImported(ScriptableObject obj)
        {
            if (_instance != null && obj is T t && obj != _instance)
            {
                Debug.LogError($"Created an asset of singleton type {typeof(T)} multiple times. "
                               + $"Please make sure there is only one asset of type {typeof(T)}");
                Resources.UnloadAsset(_instance);
                _instance = t;
            }
        }
    }
}