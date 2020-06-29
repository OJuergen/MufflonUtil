using System.Linq;
using UnityEngine;

namespace MufflonUtil
{
    public class AssetLoader : MonoBehaviour
    {
        // ReSharper disable once NotAccessedField.Local - load scriptable object assets by reference
        [SerializeField] private ScriptableObject[] _assets;

        [ContextMenu("Find Assets")]
        private void FindAssets()
        {
#if UNITY_EDITOR
            _assets = UnityEditor.AssetDatabase.FindAssets("t:ScriptableObject")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Where(path => path.StartsWith("Assets/")) // exclude package assets
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>)
                .Where(so => so is ISingleton)
                .OrderBy(so => so.name)
                .ToArray();
#endif
        }
    }
}