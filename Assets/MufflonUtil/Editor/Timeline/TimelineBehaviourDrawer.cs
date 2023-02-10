using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
    [CustomPropertyDrawer(typeof(TimelineBehaviour), true)]
    public class TimelineBehaviourDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.Next(true);
            if (property.depth == 1)
            {
                EditorGUILayout.LabelField("Behaviour", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
            }

            while (property.depth == 1)
            {
                EditorGUILayout.PropertyField(property);
                if(!property.Next(false)) break;
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.Separator();
        }
    }
}