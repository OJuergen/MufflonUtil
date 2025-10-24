using System;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MufflonUtil
{
    [Serializable]
    public struct SceneReference : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField] private SceneAsset _sceneAsset;
#endif
        [field: SerializeField, NotEditable] public String SceneName { get; private set; }
        [field: SerializeField, NotEditable] public String AssetPath { get; private set; }
        public Scene Scene => SceneManager.GetSceneByName(SceneName);
        public bool IsLoaded => Scene.isLoaded;
        [field: SerializeField, NotEditable] public bool IsValid { get; private set; }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            IsValid = _sceneAsset;
            SceneName = IsValid ? _sceneAsset.name : "";
            AssetPath = IsValid ? AssetDatabase.GetAssetPath(_sceneAsset) : "";
#endif
        }

        public void OnAfterDeserialize()
        { }

        public static implicit operator Scene(SceneReference sceneReference)
        {
            return sceneReference.Scene;
        }

        public static implicit operator string(SceneReference sceneReference)
        {
            return sceneReference.SceneName;
        }
    }
}