using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tobo.Util;
#if UNITY_EDITOR
using UnityEditor.UI;
using UnityEditor;
#endif

namespace Tobo.Util
{
    // https://discussions.unity.com/t/ui-panel-without-image-component-as-raycast-target-it-is-possible/152401/4

    /// <summary>
    /// A concrete subclass of the Unity UI `Graphic` class that just skips drawing.
    /// Useful for providing a raycast target without actually drawing anything.
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class NonDrawingGraphic : Graphic
    {
        public override void SetMaterialDirty() { return; }
        public override void SetVerticesDirty() { return; }

        // Probably not necessary since the chain of calls `Rebuild()`->`UpdateGeometry()`->`DoMeshGeneration()`->`OnPopulateMesh()` won't happen; so here really just as a fail-safe.
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            return;
        }
    }
}

#if UNITY_EDITOR

namespace Tobo.Util.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(NonDrawingGraphic), false)]
    public class NonDrawingGraphicEditor : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            base.serializedObject.Update();

            bool cacheEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.PropertyField(base.m_Script, new GUILayoutOption[0]);
            GUI.enabled = cacheEnabled;

            // skipping AppearanceControlsGUI
            base.RaycastControlsGUI();
            base.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
