using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [CustomPropertyDrawer(typeof(MarkerFromTimelineAttribute))]
    public class MarkerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var monoBehaviour = property.serializedObject.targetObject as MonoBehaviour;
            if (monoBehaviour == null)
            {
                EditorGUI.HelpBox(position, "[MarkerFromTimeline] attribute is only supported for MonoBehaviour",
                    MessageType.Error);
                return;
            }

            var playableDirector = monoBehaviour.GetComponent<PlayableDirector>();
            if (playableDirector == null)
            {
                EditorGUI.HelpBox(position, "Must attach PlayableDirector component", MessageType.Warning);
                return;
            }

            var marker = property.objectReferenceValue as Marker;
            var timelineAsset = playableDirector.playableAsset as TimelineAsset;
            if (timelineAsset == null) return;
            Marker[] markers = timelineAsset.markerTrack.GetMarkers()
                .Where(m => m is INotification || m is Marker)
                .Cast<Marker>()
                .ToArray();
            var options = new List<GUIContent> {new GUIContent("[None]")};
            options.AddRange(markers.Select(m => new GUIContent(m.name)));
            int index = Array.IndexOf(markers, marker) + 1;
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            index = EditorGUI.Popup(position, label, index, options.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                property.objectReferenceValue = index == 0 ? null : markers[index - 1];
            }

            EditorGUI.EndProperty();
        }
    }
}