using UnityEditor;
using UnityEngine.UIElements;

namespace ANGELWARE.AW_APS.Editor
{
    public class AW_BaseInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var baseInspectorUxmlPath = AssetDatabase.GUIDToAssetPath("d5b9f90ce3a624049bacf79a9308ff3d");
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(baseInspectorUxmlPath);

            // Instantiate UXML
            var root = visualTreeAsset.Instantiate();
            SetupContent(root);
            return root;
        }

        protected virtual void SetupContent(VisualElement root)
        {
            // Add default or custom content here
            // Example: root.Add(new Label("Override SetupContent to customize"));
        }
    }
}