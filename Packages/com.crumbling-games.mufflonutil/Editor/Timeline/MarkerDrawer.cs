using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil.Editor
{
    [CustomPropertyDrawer(typeof(MarkerFromTimelineAttribute))]
    public class MarkerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var clipAsset = property.serializedObject.targetObject as ClipPlayableAsset;
            if (clipAsset == null)
            {
                EditorGUI.HelpBox(position, "[MarkerFromTimeline] attribute is only supported for ClipAsset",
                    MessageType.Error);
                return;
            }

            var marker = property.objectReferenceValue as Marker;
            if (clipAsset.Clip == null || clipAsset.Clip.GetParentTrack() == null) return;
            Marker[] markers = clipAsset.Clip.GetParentTrack().GetMarkers()
                .Where(m => m is INotification || m is Marker)
                .Cast<Marker>()
                .ToArray();
            var options = new List<GUIContent> {new("[None]")};
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