using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MufflonUtil.Editor
{
    /// <summary>
    /// Custom property drawer for reactions to timeline signals. Sets the handler GameObject as target
    /// for convenience.
    /// </summary>
    [CustomPropertyDrawer(typeof(CustomSignalEventDrawer))]
    internal class SignalEventDrawer : UnityEventDrawer
    {
        private const string TargetPropertyName = "m_Target";

        private static GameObject FindBoundObject(SerializedProperty property)
        {
            var component = property.serializedObject.targetObject as Component;
            return component ? component.gameObject : null;
        }

        protected override void OnAddEvent(ReorderableList list)
        {
            base.OnAddEvent(list);
            SerializedProperty listProperty = list.serializedProperty;
            if (listProperty.arraySize <= 0) return;
            SerializedProperty lastCall = list.serializedProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
            SerializedProperty targetProperty = lastCall.FindPropertyRelative(TargetPropertyName);
            targetProperty.objectReferenceValue = FindBoundObject(listProperty);
        }

        protected override void DrawEventHeader(Rect headerRect) {}

        protected override void SetupReorderableList(ReorderableList list)
        {
            base.SetupReorderableList(list);
            list.headerHeight = 0;
        }
    }
}