using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Adobe.Substance.Editor.Importer;
using System.Text.RegularExpressions;
using System.Linq;

namespace Adobe.Substance.Editor
{
    [CustomEditor(typeof(SubstanceFileSO))]
    [CanEditMultipleObjects]
    public class SubstanceFileEditor : UnityEditor.Editor
    {
        private SubstanceFileSO _target;

        public void OnEnable()
        {
            _target = serializedObject.targetObject as SubstanceFileSO;
        }

        /// <summary>
        /// Callback for GUI events to block substance files from been duplicated.
        /// </summary>
        /// <param name="guid">Asset guid.</param>
        /// <param name="rt">GUI rect.</param>
        protected static void OnHierarchyWindowItemOnGUI(string guid, Rect rt)
        {
            var currentEvent = Event.current;

            if ("Duplicate" == currentEvent.commandName && currentEvent.type == EventType.ExecuteCommand)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);

                if (Path.GetExtension(assetPath) == ".sbsar")
                {
                    Debug.LogWarning("Substance graph can not be manually duplicated.");
                    currentEvent.Use();
                }
            }
        }

        public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
        {
            if (_target == null)
                return null;

            if (_target.Instances[0] == null)
                return null;

            if (_target.Instances[0].Graph == null)
                return null;

            if (_target.Instances[0].Graph.HasThumbnail)
            {
                var thumbnailTexture = _target.Instances[0].Graph.GetThumbnailTexture();
                return thumbnailTexture;
            }
            else
            {
                var icon = UnityPackageInfo.GetSubstanceIcon(width, height);

                if (icon != null)
                {
                    Texture2D tex = new Texture2D(width, height);
                    EditorUtility.CopySerialized(icon, tex);
                    return tex;
                }
            }

            return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }

        #region Scene Drag

        public void OnSceneDrag(SceneView sceneView, int index)
        {
            Event evt = Event.current;

            if (evt.type == EventType.Repaint)
                return;

            var materialIndex = -1;
            var go = HandleUtility.PickGameObject(evt.mousePosition, out materialIndex);

            if (_target.Instances[0].Graph.OutputMaterial != null)
            {
                if (go && go.GetComponent<Renderer>())
                    HandleRenderer(go.GetComponent<Renderer>(), materialIndex, _target.Instances[0].Graph.OutputMaterial, evt.type, evt.alt);
            }
        }

        internal static void HandleRenderer(Renderer r, int materialIndex, Material dragMaterial, EventType eventType, bool alt)
        {
            var applyMaterial = false;
            switch (eventType)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    applyMaterial = true;
                    break;

                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();
                    applyMaterial = true;
                    break;
            }
            if (applyMaterial)
            {
                var materials = r.sharedMaterials;

                bool isValidMaterialIndex = (materialIndex >= 0 && materialIndex < r.sharedMaterials.Length);
                if (!alt && isValidMaterialIndex)
                {
                    materials[materialIndex] = dragMaterial;
                }
                else
                {
                    for (int q = 0; q < materials.Length; ++q)
                        materials[q] = dragMaterial;
                }

                r.sharedMaterials = materials;
            }
        }

        #endregion Scene Drag
    }
}