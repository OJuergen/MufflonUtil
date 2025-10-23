using UnityEditor;
using UnityEngine;

namespace MufflonUtil.Editor
{
    [CustomEditor(typeof(ScriptableObject), true)]
    public class AssetManagerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (target is not IAssetManager assetManager) return;
            if (GUILayout.Button("Refresh"))
            {
                assetManager.FindAssets();
            }
        }
    }
}