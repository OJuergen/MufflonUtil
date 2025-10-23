using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace MufflonUtil.Editor
{
    /// <summary>
    /// Custom drawer for signal groups. Gathers available signal groups and provides a dropdown as well as creation utilities.
    /// </summary>
    [CustomPropertyDrawer(typeof(SignalGroupAttribute))]
    public class SignalGroupDrawer : PropertyDrawer
    {
        private static string[] _groups;
        private bool _addingNew;
        private string _newGroup;
        private GUIStyle _iconButtonStyle;
        private bool _initialized;

        private string[] FindGroups()
        {
            return AssetDatabase.FindAssets("t:TimelineAsset")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<TimelineAsset>)
                .SelectMany(timeline => timeline.GetOutputTracks())
                .SelectMany(track => track.GetMarkers())
                .OfType<Signal>()
                .Select(signal => signal.Group)
                .Where(group => !string.IsNullOrEmpty(group))
                .Distinct()
                .ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_groups == null || !_initialized)
            {
                _groups = FindGroups();
                _initialized = true;
            }

            if (!(attribute is SignalGroupAttribute signalGroupAttribute)) return;
            if (_iconButtonStyle == null) _iconButtonStyle = GUI.skin.FindStyle("IconButton");

            if (_addingNew) // create new signal group
            {
                position.width -= 50;
                _newGroup = EditorGUI.TextField(position, "New Group:", _newGroup);
                position.x += position.width + 5;
                position.width = 20;
                if (!_groups.Contains(_newGroup) && !string.IsNullOrEmpty(_newGroup) && _newGroup != "-- none --" &&
                    !_newGroup.Contains('/') && EditorGUI.DropdownButton(position,
                        EditorGUIUtility.IconContent("P4_CheckOutRemote"), FocusType.Passive, _iconButtonStyle))
                {
                    _addingNew = false;
                    property.stringValue = _newGroup;
                    _initialized = false;
                    return;
                }

                position.x += position.width + 5;
                if (EditorGUI.DropdownButton(position, EditorGUIUtility.IconContent("P4_DeletedLocal"),
                    FocusType.Passive,
                    _iconButtonStyle))
                {
                    _addingNew = false;
                }
            }
            else // present signal group dropdown
            {
                if (signalGroupAttribute.CanAddGroup) position.width -= 25;
                var options = new GUIContent[_groups.Length + 1];
                options[0] = new GUIContent("-- none --");
                Array.Copy(_groups.Select(group => new GUIContent(group)).ToArray(), 0, options, 1, _groups.Length);
                int index = Array.IndexOf(_groups, property.stringValue) + 1;
                var newLabel = new GUIContent(label)
                    {tooltip = "SignalHandlers can react to individual signals or to signal groups. Cannot contain /"};
                int newIndex = EditorGUI.Popup(position, newLabel, index, options);
                if (newIndex != index)
                {
                    property.stringValue = newIndex == 0 ? null : _groups[newIndex - 1];
                    _initialized = false;
                }

                // add new group button
                position.x += position.width + 5;
                position.width = 20;
                if (signalGroupAttribute.CanAddGroup && EditorGUI.DropdownButton(position,
                    EditorGUIUtility.IconContent("P4_AddedRemote"), FocusType.Passive, _iconButtonStyle))
                {
                    _addingNew = true;
                    _newGroup = "";
                }
            }
        }
    }
}