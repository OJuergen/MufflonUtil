using UnityEditor;
using UnityEditor.Timeline;
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
            var selectedSignal = serializedObject.context as Signal;

            SerializedProperty signalsProperty = serializedObject.FindProperty("_signals");
            SerializedProperty reactionsProperty = serializedObject.FindProperty("_reactions");
            SerializedProperty signalGroupsProperty = serializedObject.FindProperty("_signalGroups");
            SerializedProperty groupReactionsProperty = serializedObject.FindProperty("_groupReactions");

            if (signalsProperty.arraySize == 0 && signalGroupsProperty.arraySize == 0 && selectedSignal == null)
            {
                EditorGUILayout.HelpBox(
                    "No signals registered. Select a Signal in a Timeline and add a reaction from there.",
                    MessageType.Info);
                return;
            }

            EditorGUILayout.Space();

            // signal reactions
            EditorGUILayout.LabelField("Signal Reactions", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            GUILayout.BeginVertical();
            for (var i = 0; i < signalsProperty.arraySize; i++)
            {
                var signal = signalsProperty.GetArrayElementAtIndex(i).objectReferenceValue as Signal;
                if (signal == null) continue;
                if (selectedSignal != null && selectedSignal != signal) continue;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(signalsProperty.GetArrayElementAtIndex(i));
                GUI.tooltip = "test";
                if (TimelineEditor.inspectedDirector != null && GUILayout.Button(new GUIContent("Select",
                    "Sets the timeline of the PlayableDirector to the one containing this signal and brings the inspector to that signal")))
                {
                    TimelineEditor.inspectedDirector.playableAsset = signal.parent.timelineAsset;
                    Selection.objects = new Object[] {signal};
                    TimelineEditor.Refresh(RefreshReason.WindowNeedsRedraw);
                }

                if (GUILayout.Button("Remove Reaction"))
                    signalHandler.RemoveReactionTo(signal);
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.PropertyField(reactionsProperty.GetArrayElementAtIndex(i), true);
            }

            if (selectedSignal != null && !signalHandler.HasReactionTo(selectedSignal)
                                       && GUILayout.Button($"Add Reaction to Signal \"{selectedSignal.name}\""))
            {
                signalHandler.AddReactionTo(selectedSignal);
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            // group reactions
            EditorGUILayout.LabelField("Signal Group Reactions", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            GUILayout.BeginVertical();
            for (var i = 0; i < signalGroupsProperty.arraySize; i++)
            {
                string signalGroup = signalGroupsProperty.GetArrayElementAtIndex(i).stringValue;
                if (selectedSignal != null && signalGroup != selectedSignal.Group) continue;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(signalGroupsProperty.GetArrayElementAtIndex(i), GUIContent.none);
                if (GUILayout.Button("Remove Reaction"))
                    signalHandler.RemoveReactionTo(signalGroup);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(groupReactionsProperty.GetArrayElementAtIndex(i), true);
            }


            if (selectedSignal != null && !string.IsNullOrEmpty(selectedSignal.Group)
                                       && !signalHandler.HasReactionTo(selectedSignal.Group)
                                       && GUILayout.Button($"Add Reaction to Group \"{selectedSignal.Group}\""))
            {
                signalHandler.AddReactionTo(selectedSignal.Group);
            }

            if (GUILayout.Button("Add New Signal Group Reaction")) signalHandler.AddReactionTo("");
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}