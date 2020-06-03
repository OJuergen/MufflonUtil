using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MufflonUtil
{
    [CustomPropertyDrawer(typeof(CustomSignalEventDrawer))]
    internal class SignalEventDrawer : UnityEventDrawer
    {
        private const string KInstancePath = "m_Target";
        
        static GameObject FindBoundObject(SerializedProperty property)
        {
            var component = property.serializedObject.targetObject as Component;
            return component != null ? component.gameObject : null;
        }

        protected override void OnAddEvent(ReorderableList list)
        {
            base.OnAddEvent(list);
            var listProperty = list.serializedProperty;
            if (listProperty.arraySize > 0)
            {
                var lastCall = list.serializedProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
                var targetProperty = lastCall.FindPropertyRelative(KInstancePath);
                targetProperty.objectReferenceValue = FindBoundObject(listProperty);
            }
        }

        protected override void DrawEventHeader(Rect headerRect) {}

        protected override void SetupReorderableList(ReorderableList list)
        {
            base.SetupReorderableList(list);
            list.headerHeight = 4;
        }
    }
}