using UnityEditor;
using UnityEngine;

namespace MufflonUtil.Editor
{
    [CustomEditor(typeof(TimelineController))]
    public class TimelineControllerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Break Loop")) ((TimelineController) target).BreakLoop();
            if(GUILayout.Button("Jump")) ((TimelineController) target).Jump();
        }
    }
}