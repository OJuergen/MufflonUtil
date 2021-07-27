using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace MufflonUtil
{
    [CustomEditor(typeof(DataSignalReceiver), true)]
    public class DataSignalReceiverInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            Type type = target.GetType();
            while (type != null && type.BaseType != typeof(DataSignalReceiver)) type = type.BaseType;
            if (type == null) return;
            GetType().GetMethod(nameof(Render), BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(type.GetGenericArguments()[0])
                .Invoke(this, new object[0]);
        }

        private void Render<T>()
        {
            serializedObject.Update();
            var signalReceiver = target as DataSignalReceiver<T>;

            if (signalReceiver == null) return;

            SerializedProperty signalsProperty = serializedObject.FindProperty("_events._signals");
            SerializedProperty reactionsProperty = serializedObject.FindProperty("_events._events");
            
            for (var i = 0; i < signalsProperty.arraySize; i++)
            {
                var signal = signalsProperty.GetArrayElementAtIndex(i).objectReferenceValue as Signal;
                // if (signal == null) continue;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(signal, typeof(SignalAsset), false);
                EditorGUILayout.PropertyField(reactionsProperty.GetArrayElementAtIndex(i), true);
                EditorGUILayout.EndHorizontal();

                if (serializedObject.context is Signal && signal != serializedObject.context) GUI.enabled = false;
                GUI.enabled = true;
            }
            
            EditorGUILayout.Space();
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Reaction"))
                {
                    Undo.RecordObject(target, L10n.Tr("Add Signal Receiver Reaction"));
                    signalReceiver.AddEmptyReaction(new DataSignalReceiver<T>.DataUnityEvent());
                    PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                }
                GUILayout.Space(18.0f);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}