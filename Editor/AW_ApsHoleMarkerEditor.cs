﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
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
        private SerializedProperty _notOnHips;
        private SerializedProperty _ring;
        
        protected void OnEnable()
        {
            _tag = serializedObject.FindProperty("tag");
            _root = serializedObject.FindProperty("root");
            _depth = serializedObject.FindProperty("depth");
            _ring = serializedObject.FindProperty("ring");
            _haptics = serializedObject.FindProperty("haptics");
            _autoMode = serializedObject.FindProperty("autoMode");
            _notOnHips = serializedObject.FindProperty("notOnHips");
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
            var ringField = new PropertyField(_ring, "Is Ring");
            container.Add(ringField);
            var hapticsField = new PropertyField(_haptics, "Haptics");
            container.Add(hapticsField);
            var notOnHips = new PropertyField(_notOnHips, "Not on Hips");
            container.Add(notOnHips);
            var autoModeField = new PropertyField(_autoMode, "Include in Auto-Mode");
            container.Add(autoModeField);
            var selfInteractField = new PropertyField(_selfInteract, "Self Interact");
            container.Add(selfInteractField);
        }
    }
}

#endif