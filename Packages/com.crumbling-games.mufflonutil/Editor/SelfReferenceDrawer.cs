using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MufflonUtil.Editor
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
            if (!targetObject)
            {
                EditorGUI.HelpBox(position, "Invalid serialized object", MessageType.Warning);
                return;
            }

            if (!typeof(Component).IsAssignableFrom(fieldInfo.FieldType) &&
                !typeof(GameObject).IsAssignableFrom(fieldInfo.FieldType))
            {
                EditorGUI.PropertyField(position, property);
                return;
            }

            var searchInChildren = (bool) customAttributeData.ConstructorArguments[0].Value;
            var searchInParent = (bool) customAttributeData.ConstructorArguments[1].Value;
            var autoAssign = (bool) customAttributeData.ConstructorArguments[2].Value;
            label = EditorGUI.BeginProperty(position, label, property);
            Object value = property.objectReferenceValue;

            Object[] choices = GetChoices(targetObject, searchInChildren, searchInParent);
            var choiceStrings = new List<string> {"<None>"};
            for (var i = 0; i < choices.Length; i++)
            {
                string name = choices[i].name;
                if (choices.Count(c => c.name == name) == 1) choiceStrings.Add(name);
                else
                {
                    var sameNameBeforeCount = 0;
                    for (var j = 0; j < i; j++)
                        if (choices[j].name == name)
                            sameNameBeforeCount++;
                    switch (sameNameBeforeCount)
                    {
                        case 0:
                            choiceStrings.Add($"{name} ({choices[i].GetType().Name})");
                            break;
                        case 1:
                            choiceStrings.Add($"{name} (2nd {choices[i].GetType().Name})");
                            break;
                        case 2:
                            choiceStrings.Add($"{name} (3rd {choices[i].GetType().Name})");
                            break;
                        default:
                            choiceStrings.Add($"{name} ({sameNameBeforeCount + 1}th {choices[i].GetType().Name})");
                            break;
                    }
                }
            }

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(property.serializedObject.targetObject, $"Change self-reference {property.name}");

            if (autoAssign && !value && choices.Length > 0) value = choices[0];
            int selectedIndex = Array.IndexOf(choices, value) + 1;
            selectedIndex = EditorGUI.Popup(position, label, selectedIndex,
                choiceStrings.Select(choice => new GUIContent {text = choice}).ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                property.objectReferenceValue = selectedIndex == 0 ? null : choices[selectedIndex - 1];
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                PrefabUtility.RecordPrefabInstancePropertyModifications(property.serializedObject.targetObject);
                if (property.serializedObject.targetObject is GameObject go)
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(go.scene);
            }

            EditorGUI.EndProperty();
        }

        private Object[] GetChoices(Component target, bool children, bool parents)
        {
            if (typeof(Component).IsAssignableFrom(fieldInfo.FieldType))
            {
                HashSet<Component> components = target.GetComponents(fieldInfo.FieldType).MakeHashSet();
                if (children)
                    components.UnionWith(target.GetComponentsInChildren(fieldInfo.FieldType, true));
                if (parents)
                    components.UnionWith(target.GetComponentsInParent(fieldInfo.FieldType, true));
                return components.Select(c => c as Object).ToArray();
            }

            if (typeof(GameObject).IsAssignableFrom(fieldInfo.FieldType))
            {
                var components = new HashSet<GameObject> {target.gameObject};
                if (children)
                    components.UnionWith(target.GetComponentsInChildren<Transform>(true).Select(t => t.gameObject));
                if (parents)
                    components.UnionWith(target.GetComponentsInParent<Transform>(true).Select(t => t.gameObject));
                return components.Select(c => c as Object).ToArray();
            }

            throw new Exception("Illegal field type");
        }
    }
}