using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    [CustomPropertyDrawer(typeof(MinMaxInt))]
    public class MinMaxIntDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects) return 0f;
            if (fieldInfo.CustomAttributes.Any(a => a.AttributeType == typeof(MinMaxAttribute)))
                return base.GetPropertyHeight(property, label) + 20;
            else
                return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects) return;

            SerializedProperty minProperty = property.FindPropertyRelative("Min");
            SerializedProperty maxProperty = property.FindPropertyRelative("Max");

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            int min = minProperty.intValue;
            int max = maxProperty.intValue;

            var left = new Rect(position.x, position.y, position.width / 2 - 11f, 18);
            var right = new Rect(position.x + position.width - left.width, position.y, left.width, 18);
            var mid = new Rect(left.xMax, position.y, 22, 18);
            min = EditorGUI.IntField(left, min);
            EditorGUI.LabelField(mid, " to ");
            max = EditorGUI.IntField(right, max);

            CustomAttributeData customAttributeData =
                fieldInfo.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(MinMaxAttribute));
            if (customAttributeData != null)
            {
                var lowerBound = (float)customAttributeData.ConstructorArguments[0].Value;
                var upperBound = (float)customAttributeData.ConstructorArguments[1].Value;
                min = Mathf.Clamp(min, (int)lowerBound, (int)upperBound);
                max = Mathf.Clamp(max, (int)lowerBound, (int)upperBound);
                
                position.y += 18f;
                position.height = 14;
                var minFloat = (float) min;
                var maxFloat = (float) max;
                EditorGUI.MinMaxSlider(position, GUIContent.none, ref minFloat, ref maxFloat, lowerBound, upperBound);
                min = (int) minFloat;
                max = (int) maxFloat;
            }
            minProperty.intValue = min;
            maxProperty.intValue = max;
            EditorGUI.EndProperty();
        }
    }
}