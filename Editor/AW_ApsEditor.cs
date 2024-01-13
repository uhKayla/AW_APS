using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        private List<AW_ApsHoleMarker> _markers;
        private VisualTreeAsset _transformUI;
        
        protected void OnEnable()
        {
            var transforUIPath = AssetDatabase.GUIDToAssetPath("1e6b3feec2fa35142b6d263ade7bf569");
            _transformUI = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(transforUIPath);
            _markers = new List<AW_ApsHoleMarker>();

            var holeMarkers = ((AW_Aps)target).transform.GetComponentsInChildren<AW_ApsHoleMarker>();
            _markers.AddRange(holeMarkers);
        }

        protected override void SetupContent(VisualElement root)
        {
            base.SetupContent(root);

            var text = new Label(
                "This component is meant to be added to the root of the avatar, and handles the finding and construction of all holes. This is for improved performance, as opposed to baking each hole one-by-one.");
            
            var container = root.Q<VisualElement>("Container");

            text.style.flexWrap = Wrap.Wrap;
            text.style.whiteSpace = WhiteSpace.Normal;
            container.Add(text);

            if (_markers.Count < 0)
            {
                Debug.Log("No markers could be found in this heirarchy.");
            }
            
            foreach (var marker in _markers)
            {
                var holeMarker = new AW_ApsTransforms((AW_ApsHoleMarker)marker, ((AW_Aps)target).transform);
                var holeUI = _transformUI.Instantiate();
                
                var textField = holeUI.Q<Label>("HoleName");
                var objectField = holeUI.Q<ObjectField>("Bone");
                var selectButton = holeUI.Q<ToolbarButton>("SelectButton");

                textField.text = holeMarker.holeMarker.name;
                
                selectButton.RegisterCallback<MouseUpEvent>(evt =>
                {
                    EditorGUIUtility.PingObject(marker.transform);
                });
                
                container.Add(holeUI);
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