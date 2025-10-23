using JetBrains.Annotations;
using UnityEngine;

namespace MufflonUtil
{
    /// <summary>
    /// Extension of <see cref="ScriptableObject"/> that stores the file GUID as a serialized field and exposes it as
    /// a public property <see cref="GUID"/>.
    /// This can be useful for identifying an asset over the network or from a generated file or database.
    /// </summary>
    public class ScriptableObjectWithGUID : ScriptableObject, ISerializationCallbackReceiver
    {
        /// <summary>
        /// The Unity-generated file GUID. This is a unique identifier for the asset.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [field: SerializeField, NotEditable] public string GUID { get; [UsedImplicitly] private set; }
        
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            GUID = UnityEditor.AssetDatabase.AssetPathToGUID(UnityEditor.AssetDatabase.GetAssetPath(this));
#endif
        }

        public void OnAfterDeserialize()
        { }
    }
}