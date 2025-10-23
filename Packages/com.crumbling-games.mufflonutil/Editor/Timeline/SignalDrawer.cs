using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace MufflonUtil.Editor
{
    /// <summary>
    /// Custom drawer for signals. Gathers available signals from timelines in the project and offers a popup dropdown.
    /// </summary>
    [CustomPropertyDrawer(typeof(Signal), true)]
    public class SignalDrawer : PropertyDrawer
    {
        private Signal[] _signals;

        private Signal[] FindSignals(Type type)
        {
            return AssetDatabase
                .FindAssets("t:TimelineAsset")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<TimelineAsset>)
                .SelectMany(timeline => timeline.GetOutputTracks())
                .SelectMany(track => track.GetMarkers())
                .Where(type.IsInstanceOfType)
                .OfType<Signal>()
                .ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var signal = property.objectReferenceValue as Signal;
            if (signal == null) return;

            if (_signals == null)
            {
                // Try to limit the type of proposed signals to the assignable type
                var signalHandler = property.serializedObject.targetObject as SignalHandlerBase;
                _signals = FindSignals(signalHandler == null ? typeof(Signal) : signalHandler.SignalType);
            }

            int index = Array.IndexOf(_signals, signal);
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            int newIndex = EditorGUI.Popup(position, index, _signals.Select(s => new GUIContent(
                    $"{s.parent.timelineAsset.name}: {s.name.Replace("/", "|")} (t={s.time:0.##}s)"))
                .ToArray());
            EditorGUI.EndProperty();
            if (newIndex != index) property.objectReferenceValue = _signals[newIndex];
        }
    }
}