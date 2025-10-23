using UnityEditor;
using UnityEngine;

namespace MufflonUtil.Editor
{
    [CustomEditor(typeof(Comment))]
    public class CommentInspector : UnityEditor.Editor
    {
        private SerializedProperty _name;
        private SerializedProperty _description;
        private bool _isEditingName;
        private bool _isEditingDescription;

        private void OnEnable()
        {
            var comment = target as Comment;
            if (comment == null) return;

            _name = serializedObject.FindProperty("_name");
            _description = serializedObject.FindProperty("_description");

            // rearrange components to make description always be on top
            if (PrefabUtility.GetPrefabInstanceStatus(comment.gameObject) != PrefabInstanceStatus.Connected)
            {
                while (UnityEditorInternal.ComponentUtility.MoveComponentUp(comment))
                { }
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            if (string.IsNullOrEmpty(_name.stringValue)) _name.stringValue = target.name;
            var centeredBoldLabel = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20
            };
            if (_isEditingName) _name.stringValue = EditorGUILayout.TextField(_name.stringValue, centeredBoldLabel);
            else EditorGUILayout.LabelField(_name.stringValue, centeredBoldLabel);
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_editicon.sml"), GUILayout.Width(32)))
            {
                _isEditingName = !_isEditingName;
            }
            

            EditorGUILayout.EndHorizontal();
            Rect rect = EditorGUILayout.GetControlRect(false, 2 );
            rect.height = 2;
            EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
            EditorGUILayout.Separator();

            // description
            EditorGUILayout.BeginHorizontal();
            var descriptionStyle = new GUIStyle(GUI.skin.label)
            {
                wordWrap = true,
                richText = true
            };
            if (_isEditingDescription)
            {
                _description.stringValue = EditorGUILayout.TextArea(_description.stringValue, descriptionStyle);
            }
            else
            {
                EditorGUILayout.LabelField(_description.stringValue, descriptionStyle);
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("d_editicon.sml"), GUILayout.Width(32)))
            {
                _isEditingDescription = !_isEditingDescription;
            }

            EditorGUILayout.EndHorizontal();

            // end
            serializedObject.ApplyModifiedProperties();
        }
    }
}