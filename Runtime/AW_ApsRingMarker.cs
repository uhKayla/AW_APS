using System;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;

namespace ANGELWARE.AW_APS
{
    [AddComponentMenu("ANGELWARE/APS/Hole Marker")]
    [HelpURL("https://angelware.info/aps/holes/")]
    public class AW_ApsRingMarker : MonoBehaviour, IEditorOnly
    {
        public Transform root;
        public string tag;
        public bool haptics;
        public bool selfInteract;
        public bool autoMode;
        public bool notOnHips;
        private Color color;
        public Transform rootTransform;
        
        private Mesh _gizmoMesh;

        private void OnDrawGizmosSelected()
        {
            if (_gizmoMesh == null)
            {
                var gizmoPath = AssetDatabase.GUIDToAssetPath("467c00be67ec2594590087d656090efa");
                _gizmoMesh = AssetDatabase.LoadAssetAtPath<Mesh>(gizmoPath);
            }

            if (_gizmoMesh != null)
            {
                color = new Color(231, 64, 67);
                Gizmos.color = color;
                Gizmos.DrawMesh(_gizmoMesh, transform.position, transform.rotation, new Vector3(0.15f,0.15f,0.15f));
            }
        } 
    }
}