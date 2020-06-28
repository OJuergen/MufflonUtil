using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MufflonUtil
{
    [CustomPropertyDrawer(typeof(SelfReferenceAttribute))]
    public class SelfReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CustomAttributeData customAttributeData = fieldInfo.CustomAttributes
                .FirstOrDefault(a => a.AttributeType == typeof(SelfReferenceAttribute));
            if (customAttributeData == null)
            {
                EditorGUI.HelpBox(position, "Missing attribute data", MessageType.Warning);
                return;
            }

            var go = property.objectReferenceValue as GameObject;
            Debug.Log($"parent is maybe {go}");
            var targetObject = property.serializedObject.targetObject as Component;
            if (targetObject == null)
            {
                EditorGUI.HelpBox(position, "Invalid serialized object", MessageType.Warning);
                return;
            }

            var searchInChildren = (bool) customAttributeData.ConstructorArguments[0].Value;
            var searchInParent = (bool) customAttributeData.ConstructorArguments[1].Value;

            HashSet<Component> components = targetObject.GetComponents(fieldInfo.FieldType).ToHashSet();
            if (searchInChildren)
                components.UnionWith(targetObject.GetComponentsInChildren(fieldInfo.FieldType));
            if (searchInParent)
                components.UnionWith(targetObject.GetComponentsInParent(fieldInfo.FieldType));
            Object[] choices = components.Select(c => c as Object).ToArray();

            if (choices.Length > 0)
            {
                if (property.objectReferenceValue == null || !choices.Contains(property.objectReferenceValue))
                    property.objectReferenceValue = choices[0];
                int selectedIndex = Array.IndexOf(choices, property.objectReferenceValue);
                selectedIndex = EditorGUI.Popup(position, property.name, selectedIndex,
                    choices.Select(choice => choice.name).ToArray());
                property.objectReferenceValue = choices[selectedIndex];
            }
            else
            {
                EditorGUI.HelpBox(position, $"{property.name}: Missing {fieldInfo.FieldType}", MessageType.Warning);
            }
        }
    }
}