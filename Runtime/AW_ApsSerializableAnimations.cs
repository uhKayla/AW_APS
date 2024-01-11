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
        public AW_SerializableAnimations[] animations;
        public string parameterName;
    }
    
    [Serializable]
    public class AW_SerializableAnimations
    {
        public AnimationClip animations;
        public float trigger;
    }
}
#endif