using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    [CustomPropertyDrawer(typeof(ConditionallyEditableAttribute))]
    public class ConditionallyEditableDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            (bool editable, string error) = CheckProperty(property);
            if (error != null)
            {
                EditorGUI.HelpBox(position, error, MessageType.Warning);
                return;
            }
            
            EditorGUI.BeginDisabledGroup(!editable);
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndDisabledGroup();
        }

        private (bool editable, string error) CheckProperty(SerializedProperty property)
        {
            CustomAttributeData customAttributeData = fieldInfo.CustomAttributes
                .FirstOrDefault(a => a.AttributeType == typeof(ConditionallyEditableAttribute));
            if (customAttributeData == null)
            {
                return (true, "Missing attribute data");
            }

            Object targetObject = property.serializedObject.targetObject;
            if (targetObject == null)
            {
                return (true, "Invalid serialized object");
            }

            var constructorArguments = customAttributeData.ConstructorArguments;
            var fieldOrPropertyName = (string) constructorArguments[0].Value;
            object valueToCompareWith = constructorArguments.Count < 2 ? true : constructorArguments[1].Value;
            bool invert = constructorArguments.Count >= 3 && (bool) constructorArguments[2].Value;

            FieldInfo dependencyFieldInfo = targetObject.GetType().GetField(fieldOrPropertyName,
                BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (dependencyFieldInfo != null)
            {
                if (!invert && Equals(dependencyFieldInfo.GetValue(targetObject), valueToCompareWith) || invert)
                {
                    return (true, null);
                }
            }

            PropertyInfo dependencyPropertyInfo = targetObject.GetType().GetProperty(fieldOrPropertyName,
                BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (dependencyPropertyInfo != null)
            {
                if (!invert && Equals(dependencyPropertyInfo.GetValue(targetObject), valueToCompareWith) || invert)
                {
                    return (true, null);
                }
            }

            return (false, null);
        }
    }
}