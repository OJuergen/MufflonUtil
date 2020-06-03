using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    [CustomPropertyDrawer(typeof(ComponentReferenceAttribute))]
    public class ComponentReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CustomAttributeData customAttributeData = fieldInfo.CustomAttributes
                .FirstOrDefault(a => a.AttributeType == typeof(ComponentReferenceAttribute));
            if (customAttributeData == null)
            {
                EditorGUI.HelpBox(position, "Missing attribute data", MessageType.Warning);
                return;
            }

            var targetObject = property.serializedObject.targetObject as Component;
            if (targetObject == null)
            {
                EditorGUI.HelpBox(position, "Invalid serialized object", MessageType.Warning);
                return;
            }

            var searchInChildren = (bool) customAttributeData.ConstructorArguments[0].Value;
            var searchInParent = (bool) customAttributeData.ConstructorArguments[1].Value;

            var component = property.objectReferenceValue as Component;
            if (component == null) component = targetObject.GetComponent(fieldInfo.FieldType);
            if (component == null && searchInChildren)
                component = targetObject.GetComponentInChildren(fieldInfo.FieldType);
            if (component == null && searchInParent)
                component = targetObject.GetComponentInParent(fieldInfo.FieldType);

            if (component != null)
            {
                property.objectReferenceValue = component;
                bool wasEnabled = GUI.enabled;
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = wasEnabled;
            }
            else
                EditorGUI.HelpBox(position, $"Missing {fieldInfo.FieldType}", MessageType.Warning);
        }
    }
}