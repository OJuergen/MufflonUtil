using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    [CustomEditor(typeof(ScriptableObject), true)]
    public class AssetManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!(target is IAssetManager assetManager)) return;
            if (GUILayout.Button("Refresh"))
            {
                assetManager.FindAssets();
            }
        }
    }
}