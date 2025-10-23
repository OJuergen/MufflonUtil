using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MufflonUtil.Editor
{
    [CustomPropertyDrawer(typeof(ConditionallyVisibleAttribute))]
    public class ConditionallyVisibleDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return CheckProperty(property).visible ? EditorGUIUtility.singleLineHeight : 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            (bool visible, string error) = CheckProperty(property);
            if (!visible) return;
            if (error != null)
            {
                EditorGUI.HelpBox(position, error, MessageType.Warning);
                return;
            }

            EditorGUI.PropertyField(position, property, label);
        }

        private (bool visible, string error) CheckProperty(SerializedProperty property)
        {
            CustomAttributeData customAttributeData = fieldInfo.CustomAttributes
                .FirstOrDefault(a => a.AttributeType == typeof(ConditionallyVisibleAttribute));
            if (customAttributeData == null)
            {
                return (true, "Missing attribute data");
            }

            Object targetObject = property.serializedObject.targetObject;
            if (!targetObject)
            {
                return (true, "Invalid serialized object");
            }

            IList<CustomAttributeTypedArgument> constructorArguments = customAttributeData.ConstructorArguments;
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