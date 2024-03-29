﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;

namespace ANGELWARE.AW_APS
{
    [AddComponentMenu("ANGELWARE/APS/Hole Marker")]
    public class AW_ApsHoleMarker : MonoBehaviour, IEditorOnly
    {
        public Transform root;
        public string tag;
        public float depth;
        public bool haptics;
        public bool selfInteract;
        public bool autoMode;
        public bool notOnHips;
        public bool ring;
        private Color color;
        
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
                var gTransform = transform;
                Gizmos.color = color;
                Gizmos.DrawMesh(_gizmoMesh, gTransform.position, gTransform.rotation, new Vector3(0.15f,0.15f,0.15f));
            }
        }

        private void OnEnable()
        {
            root = this.transform;
        }
    }
}
#endif