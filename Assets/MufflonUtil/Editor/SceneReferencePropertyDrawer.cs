using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sceneAssetProperty = property.FindPropertyRelative("_sceneAsset");
            EditorGUI.PropertyField(rect, sceneAssetProperty, label);
        }
    }
}