using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.SDKBase;

namespace ANGELWARE.AW_APS.Editor
{
    [CustomEditor(typeof(AW_Aps))]
    public class AW_ApsEditor : AW_BaseInspector, IEditorOnly
    {
        private SerializedProperty _markers;
        public Transform transform;
        private VisualTreeAsset _transformUI;
        
        protected void OnEnable()
        {
            var transforUIPath = AssetDatabase.GUIDToAssetPath("1e6b3feec2fa35142b6d263ade7bf569");
            _transformUI = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(transforUIPath);
            _markers = serializedObject.FindProperty("markers");
        }

        protected override void SetupContent(VisualElement root)
        {
            base.SetupContent(root);
            var container = root.Q<VisualElement>("Container");

            foreach (var marker in _markers)
            {
                var holeMarker = new AW_ApsTransforms((AW_ApsHoleMarker)marker, transform);
                var holeUI = _transformUI.Instantiate();
                container.Add(holeUI);
                var textField = holeUI.Q<TextField>("HoleName");
                var objectField = holeUI.Q<ObjectField>("Bone");
                var selectButton = holeUI.Q<ToolbarButton>("SelectButton");

                holeMarker.holeMarker.tag = textField.text;
                
                objectField.objectType = typeof(Transform);
                objectField.RegisterValueChangedCallback(evt =>
                {
                    holeMarker.transform = (Transform)evt.newValue;
                });
            }
        }
    }

    [Serializable]
    class AW_ApsTransforms
    {
        public AW_ApsHoleMarker holeMarker;
        public Transform transform;

        public AW_ApsTransforms(AW_ApsHoleMarker holeMarker, Transform transform)
        {
            this.holeMarker = holeMarker;
            this.transform = transform;
        }
    }
}