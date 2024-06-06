using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            bool isValueMissing = property.propertyType == SerializedPropertyType.ObjectReference &&
                                  property.objectReferenceValue == null;
            if (isValueMissing)
            {
                Color colorCache = GUI.color;
                GUI.color = Color.yellow;
                Texture warningIcon = EditorGUIUtility.IconContent("console.warnicon").image;
                var warningLabel = new GUIContent(label)
                    { image = warningIcon, tooltip = $"[VALUE REQUIRED!]\n{label.tooltip}" };
                EditorGUI.PropertyField(rect, property, warningLabel);
                GUI.color = colorCache;
            }
            else
            {
                EditorGUI.PropertyField(rect, property, label);
            }
        }
    }
}