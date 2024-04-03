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

        [Header("Penetration System")] 
        public AnimationClip frotEnable;
        public AnimationClip frotDisable;
        
        [Header("Erection")] public AW_ApsSerializableAnimations[] erectionAnimations;
        
        [Header("Wetness")] 
        public float wetnessSpeed = 0.5f;
        public AW_ApsSerializableAnimations[] wetnessAnimations;

        [Header("Skin Movement")] 
        public float skinMovementSpeed = 2f;
        public AW_ApsSerializableAnimations[] skinMovementAnimations;

        [Header("Sizes")] 
        public AW_ApsSerializableAnimations[] sizeAnimations;
        public AW_ApsSerializableAnimations[] ballsSizeAnimations;

        [Header("Other Appearance Settings")] 
        public AW_ApsSerializableAnimations[] foreskinAnimations;
        public AW_ApsSerializableAnimations[] shaftAppearanceAnimations;
        public AW_ApsSerializableAnimations[] ballsAppearanceAnimations;

        [Header("Idle Movement")] public AnimationClip idleMovementAnimation;

        [Header("Cum")] 
        public bool enableCumEffects;
        public GameObject cumObject;
        public GameObject staticCumObject;
        public GameObject ocsFinishSender;
    }
}
#endif