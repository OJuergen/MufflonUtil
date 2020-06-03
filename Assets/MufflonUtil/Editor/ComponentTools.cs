using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MufflonUtil
{
    public static class ComponentTools
    {
        [MenuItem("CONTEXT/Component/Collapse All")]
        private static void CollapseAll(MenuCommand command)
        {
            SetAllInspectorsExpanded((command.context as Component)?.gameObject, false);
        }
 
        [MenuItem("CONTEXT/Component/Expand All")]
        private static void ExpandAll(MenuCommand command)
        {
            SetAllInspectorsExpanded((command.context as Component)?.gameObject, true);
        }

        private static void SetAllInspectorsExpanded(GameObject go, bool expanded)
        {
            var components = go.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component is Renderer renderer)
                {
                    var materials = renderer.sharedMaterials;
                    foreach (Material material in materials)
                    {
                        InternalEditorUtility.SetIsInspectorExpanded(material, expanded);
                    }
                }
                InternalEditorUtility.SetIsInspectorExpanded(component, expanded);
            }
            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }
    }
}