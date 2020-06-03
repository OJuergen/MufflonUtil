using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    [CustomEditor(typeof(SignalHandlerBase), true)]
    public class SignalHandlerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var signalHandler = target as SignalHandlerBase;
            if (signalHandler == null) return;

            SerializedProperty signalsProperty = serializedObject.FindProperty("_signals");
            SerializedProperty reactionsProperty = serializedObject.FindProperty("_reactions");

            if (signalsProperty.arraySize == 0 && !(serializedObject.context is Signal))
                EditorGUILayout.HelpBox(
                    "No signals registered. Select a Signal in a Timeline and add a reaction from there.",
                    MessageType.Info);

            EditorGUILayout.Space();
            for (var i = 0; i < signalsProperty.arraySize; i++)
            {
                var signal = signalsProperty.GetArrayElementAtIndex(i).objectReferenceValue as Signal;
                if (signal == null) continue;
                EditorGUILayout.BeginHorizontal();
                GUI.enabled = false;
                EditorGUILayout.ObjectField(signal, signal.GetType(), false);
                GUI.enabled = true;
                if (GUILayout.Button("Remove Reaction"))
                    signalHandler.RemoveReactionTo(signal);
                EditorGUILayout.EndHorizontal();
                
                if (serializedObject.context is Signal && signal != serializedObject.context) GUI.enabled = false;
                EditorGUILayout.PropertyField(reactionsProperty.GetArrayElementAtIndex(i), true);
                GUI.enabled = true;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}