using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    [CustomEditor(typeof(TimelineController))]
    public class TimelineControllerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Break Loop")) ((TimelineController) target).BreakLoop();
            if(GUILayout.Button("Jump")) ((TimelineController) target).Jump();
        }
    }
}