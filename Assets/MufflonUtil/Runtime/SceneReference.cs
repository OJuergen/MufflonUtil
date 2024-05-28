using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MufflonUtil
{
    [Serializable]
    public struct SceneReference : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField] private UnityEditor.SceneAsset _sceneAsset;
#endif
        [SerializeField, NotEditable] private string _sceneName;
        public string SceneName => _sceneName;
        public Scene Scene => SceneManager.GetSceneByName(_sceneName);
        public bool IsLoaded => Scene.isLoaded;
        [field: SerializeField, NotEditable] public bool IsValid { get; private set; }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            _sceneName = _sceneAsset == null ? "" : _sceneAsset.name;
            IsValid = _sceneAsset != null;
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