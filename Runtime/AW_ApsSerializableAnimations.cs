#if UNITY_EDITOR
using System;
using UnityEngine;

namespace ANGELWARE.AW_APS
{
    [Serializable]
    public class AW_ApsSerializableAnimations
    {
        public AnimationClip animation;
        public float trigger;
    }
    
    [Serializable]
    public class AW_SerializableAnimationGroup
    {
        public SerializableOutfitAnimations[] animations;
        public string parameterName;
    }
    
    [Serializable]
    public class SerializableOutfitAnimations
    {
        public AnimationClip animations;
        public float trigger;
    }
}
#endif