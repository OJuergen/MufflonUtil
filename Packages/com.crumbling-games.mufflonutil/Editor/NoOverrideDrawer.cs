using UnityEditor;
using UnityEngine;

namespace MufflonUtil.Editor
{
    [CustomPropertyDrawer(typeof(NoOverrideAttribute))]
    public class NoOverrideDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position,
                                   SerializedProperty property,
                                   GUIContent label)
        {
            bool wasEnabled = GUI.enabled;
            if (PrefabUtility.IsPartOfVariantPrefab(property.serializedObject.targetObject)
                && !PrefabUtility.IsAddedComponentOverride(property.serializedObject.targetObject))
            {
                PrefabUtility.RevertPropertyOverride(property, InteractionMode.AutomatedAction);
                GUI.enabled = false;
            }

            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = wasEnabled;
        }
    }
}