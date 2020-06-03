using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    [CustomPropertyDrawer(typeof(AnimatorParameterId))]
    public class AnimatorParameterIdDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects) return 0f;
            return 18f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects) return;
            SerializedProperty nameProperty = property.FindPropertyRelative("_name");
            string name = EditorGUI.TextField(position, label, nameProperty.stringValue);
            property.serializedObject.targetObject.GetType()
                .GetField(property.propertyPath, BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(property.serializedObject.targetObject, new AnimatorParameterId(name));
        }
    }
}