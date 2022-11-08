using System.Linq;
using UnityEngine;

namespace MufflonUtil
{
    /// <summary>
    /// Utility to load scriptable object singletons via reference.
    /// Add an object with this script to the start scene and use the FindAssets function from the context menu
    /// to generate a list of scriptable object singleton references.
    /// Good practice is to make the asset loader a prefab and call the FindAssets function from
    /// your custom build pipeline.  
    /// </summary>
    public class AssetLoader : MonoBehaviour
    {
        // ReSharper disable once NotAccessedField.Local - load scriptable object assets by reference
        [SerializeField] private ScriptableObject[] _assets;

#if UNITY_EDITOR
        [ContextMenu("Find Assets")]
        public void FindAssets()
        {
            _assets = UnityEditor.AssetDatabase.FindAssets("t:ScriptableObject")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Where(path => path.StartsWith("Assets/")) // exclude package assets
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>)
                .Where(so => so is ISingleton)
                .OrderBy(so => so.name)
                .ToArray();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}