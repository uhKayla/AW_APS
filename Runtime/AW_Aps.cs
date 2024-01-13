using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace ANGELWARE.AW_APS
{
    public class AW_Aps : MonoBehaviour, IEditorOnly
    {
        public List<AW_ApsHoleMarker> markers;
        public Transform root;
    }
}