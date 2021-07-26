using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [CustomEditor(typeof(ControlPlayableAsset))]
    public class ControlPlayableExtendedInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var playableAsset = target as ControlPlayableAsset;
            if (playableAsset == null) return;

            // hack to reuse internal clip inspector
            var type = Type.GetType(
                "UnityEditor.Timeline.ControlPlayableInspector, Unity.Timeline.Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            CreateEditorWithContext(new[] {target}, serializedObject.context, type).OnInspectorGUI();

            var inspectedDirector = serializedObject.context as PlayableDirector;
            if (inspectedDirector == null) return;
            GameObject sourceGO = playableAsset.sourceGameObject.Resolve(inspectedDirector);

            Color color = GUI.color;
            GUI.color = Color.cyan;
            if (playableAsset.prefabGameObject == null && sourceGO != null && GUILayout.Button("Convert to Prefab"))
            {
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(sourceGO);
                if (prefab == null)
                {
                    Debug.Log("Source gameObject not a prefab: Creating new prefab asset...");
                    string timelinePath = AssetDatabase.GetAssetPath(inspectedDirector.playableAsset);
                    string assetPath = timelinePath.Substring(0, timelinePath.LastIndexOf('/'));
                    string path = EditorUtility.SaveFilePanel("Create new prefab", assetPath, "ControlTrackPrefab", "prefab");
                    path = $"Assets/{path.Substring(Application.dataPath.Length)}";
                    if (string.IsNullOrEmpty(path)) return; // aborted
                    prefab = PrefabUtility.SaveAsPrefabAsset(sourceGO, path);
                }

                playableAsset.prefabGameObject = prefab;
                GameObject parentGO = sourceGO.transform.parent.gameObject;
                inspectedDirector.SetReferenceValue(playableAsset.sourceGameObject.exposedName, parentGO);
                DestroyImmediate(sourceGO);
                EditorUtility.SetDirty(playableAsset);
                EditorUtility.SetDirty(inspectedDirector);
            }

            if (playableAsset.prefabGameObject != null && GUILayout.Button("Convert to Embedded"))
            {
                Transform parent = sourceGO == null ? null : sourceGO.transform;
                var instance = (GameObject) PrefabUtility.InstantiatePrefab(playableAsset.prefabGameObject, parent);
                inspectedDirector.SetReferenceValue(playableAsset.sourceGameObject.exposedName, instance);
                playableAsset.prefabGameObject = null;
                EditorUtility.SetDirty(playableAsset);
                EditorUtility.SetDirty(inspectedDirector);
            }

            GUI.color = color;
        }
    }
}