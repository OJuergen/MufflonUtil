using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace MufflonUtil.Editor
{
    [CustomEditor(typeof(Signal), true)]
    public class SignalInspector : UnityEditor.Editor
    {
        private readonly Dictionary<Component, bool> _foldouts = new();
        private readonly Dictionary<Component, UnityEditor.Editor> _editors = new();
        public static readonly GUIContent ObjectLabel = EditorGUIUtility.TrTextContent("Receiver Component on",
            "The Signal Receiver Component on the bound GameObject.");
        private GameObject _boundGameObject;

        public override void OnInspectorGUI()
        {
            var signal = target as Signal;
            if (signal == null) return;
            signal.name = GUILayout.TextField(signal.name);
            base.OnInspectorGUI();

            PlayableDirector inspectedDirector = TimelineEditor.inspectedDirector;
            if (inspectedDirector == null)
            {
                EditorGUILayout.HelpBox("Select Playable Director to setup signal response", MessageType.Info);
                return;
            }
            if (signal.parent == signal.parent.timelineAsset.markerTrack)
                _boundGameObject = inspectedDirector.gameObject;
            else
            {
                Object genericBinding = inspectedDirector.GetGenericBinding(signal.parent);
                if(genericBinding is GameObject gameObject) _boundGameObject = gameObject;
                if(genericBinding is Component component) _boundGameObject = component.gameObject;
            }
            if (_boundGameObject == null) return;

            SignalHandlerBase[] signalHandlers = _boundGameObject.GetComponents<SignalHandlerBase>()
                .Where(handler => handler.SignalType.IsInstanceOfType(signal))
                .ToArray();
            if (signalHandlers.Length == 0 && GUILayout.Button("Add Signal Handler"))
            {
                _boundGameObject.gameObject.AddComponent<SignalHandler>();
            }
            
            foreach (SignalHandlerBase signalHandler in signalHandlers)
            {
                EditorGUILayout.Separator();
                UnityEditor.Editor editor = GetOrCreateEditor(signalHandler);
                if (DrawReceiverHeader(signalHandler)) editor.OnInspectorGUI();
            }
        }

        private UnityEditor.Editor GetOrCreateEditor(Component c)
        {
            if (_editors.TryGetValue(c, out UnityEditor.Editor editor))
            {
                return editor;
            }

            editor = CreateEditorWithContext(new Object[] {c}, target);
            _editors[c] = editor;
            if (!_foldouts.ContainsKey(c))
            {
                _foldouts.Add(c, true);
            }

            return editor;
        }

        bool DrawReceiverHeader(Component receiver)
        {
            EditorGUILayout.Space();

            var style = EditorGUIUtility.TrTextContentWithIcon(
                ObjectNames.NicifyVariableName(receiver.GetType().Name),
                AssetPreview.GetMiniThumbnail(receiver));

            _foldouts[receiver] =
                EditorGUILayout.Foldout(_foldouts[receiver], style, true, new GUIStyle(EditorStyles.foldout) {fontStyle = FontStyle.Bold});
            if (_foldouts[receiver])
            {
                DrawReceiverObjectField();
            }

            return _foldouts[receiver];
        }

        void DrawReceiverObjectField()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(ObjectLabel, _boundGameObject, typeof(Component), false);
            EditorGUI.EndDisabledGroup();
        }
    }
}