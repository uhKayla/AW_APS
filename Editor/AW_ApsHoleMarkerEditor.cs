using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.SDKBase;

namespace ANGELWARE.AW_APS.Editor
{
    [CustomEditor(typeof(AW_ApsHoleMarker))]
    public class AW_AvatarEditor : AW_BaseInspector, IEditorOnly
    {
        private SerializedProperty _tag;
        private SerializedProperty _root;
        private SerializedProperty _depth;
        private SerializedProperty _haptics;
        private SerializedProperty _autoMode;
        private SerializedProperty _selfInteract;
        
        protected void OnEnable()
        {
            _tag = serializedObject.FindProperty("tag");
            _root = serializedObject.FindProperty("root");
            _depth = serializedObject.FindProperty("depth");
            _haptics = serializedObject.FindProperty("haptics");
            _autoMode = serializedObject.FindProperty("autoMode");
            _selfInteract = serializedObject.FindProperty("selfInteract");
        }

        protected override void SetupContent(VisualElement root)
        {
            base.SetupContent(root);
            var container = root.Q<VisualElement>("Container");

            var rootField = new PropertyField(_root, "Root");
            container.Add(rootField);
            var tagField = new PropertyField(_tag, "Tag");
            container.Add(tagField);
            var depthField = new PropertyField(_depth, "Depth");
            container.Add(depthField);
            var hapticsField = new PropertyField(_haptics, "Haptics");
            container.Add(hapticsField);
            var autoModeField = new PropertyField(_autoMode, "Include in Auto-Mode");
            container.Add(autoModeField);
            var selfInteractField = new PropertyField(_selfInteract, "Self Interact");
            container.Add(selfInteractField);
        }
    }
}
