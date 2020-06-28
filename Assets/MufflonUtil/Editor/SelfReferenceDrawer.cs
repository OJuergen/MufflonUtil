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

            var targetObject = property.serializedObject.targetObject as Component;
            if (targetObject == null)
            {
                EditorGUI.HelpBox(position, "Invalid serialized object", MessageType.Warning);
                return;
            }

            if (!typeof(Component).IsAssignableFrom(fieldInfo.FieldType))
            {
                EditorGUI.PropertyField(position, property);
                return;
            }

            var searchInChildren = (bool) customAttributeData.ConstructorArguments[0].Value;
            var searchInParent = (bool) customAttributeData.ConstructorArguments[1].Value;

            HashSet<Component> components = targetObject.GetComponents(fieldInfo.FieldType).ToHashSet();
            if (searchInChildren)
                components.UnionWith(targetObject.GetComponentsInChildren(fieldInfo.FieldType, true));
            if (searchInParent)
                components.UnionWith(targetObject.GetComponentsInParent(fieldInfo.FieldType, true));
            Object[] choices = components.Select(c => c as Object).ToArray();

            if (choices.Length > 0)
            {
                label = EditorGUI.BeginProperty(position, label, property);
                EditorGUI.BeginChangeCheck();
                Undo.RecordObject(property.serializedObject.targetObject, $"Change self-reference {property.name}");

                if (property.objectReferenceValue == null || !choices.Contains(property.objectReferenceValue))
                    property.objectReferenceValue = choices[0];
                int selectedIndex = EditorGUI.Popup(position, label,
                    Array.IndexOf(choices, property.objectReferenceValue),
                    choices.Select(choice => new GUIContent {text = choice.name}).ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    property.objectReferenceValue = choices[selectedIndex];
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(property.serializedObject.targetObject);
                    if (property.serializedObject.targetObject is GameObject go)
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(go.scene);
                }

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.HelpBox(position, $"{property.name}: Missing {fieldInfo.FieldType}", MessageType.Warning);
            }
        }
    }
}