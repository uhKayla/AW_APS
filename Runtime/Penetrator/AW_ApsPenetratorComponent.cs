#if UNITY_EDITOR
using UnityEngine;
using VRC.SDKBase;

namespace ANGELWARE.AW_APS
{
    public class AW_ApsPenetratorComponent : MonoBehaviour, IEditorOnly
    {
        public Transform rootTransform;
        public Transform tipTransform;
        public string oneFloatParameter = "Internal/Float";
        public string smoothingMultiplierParam = "Internal/MultipliedSmooth";
        public string parameterName = "Penetrator";
        
        [Header("Wetness")] public AW_ApsSerializableAnimations[] wetnessAnimations;

        [Header("Idle Movement")] public AnimationClip idleMovementAnimation;
    }
}
#endif