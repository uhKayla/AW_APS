#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using AnimatorAsCode.V1;
using AnimatorAsCode.V1.NDMFProcessor;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace ANGELWARE.AW_AAC.Plugin
{
    /// <summary>
    /// Based on the original AAC plugin, this will merge animators AFTER the other plugins.
    /// I believe this is okay as it requires the original code to function.
    /// https://github.com/hai-vr/animator-as-code-ndmf-processor/blob/main/Packages/dev.hai-vr.animator-as-code.v1.ndmf-processor/V1/Editor/AacPlugin.cs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [AddComponentMenu("ANGELWARE/AAC/ApsMergerPlugin")]

    public class ApsMergerPlugin<T> : Plugin<ApsMergerPlugin<T>> where T : MonoBehaviour
    {
        // Can be changed if necessary
        private string SystemName(T script, BuildContext ctx) => typeof(T).Name;
        private Transform AnimatorRoot(T script, BuildContext ctx) => ctx.AvatarRootTransform;
        private Transform DefaultValueRoot(T script, BuildContext ctx) => ctx.AvatarRootTransform;
        protected virtual bool UseWriteDefaults(T script, BuildContext ctx) => false;

        // This state is short-lived, it's really just sugar
        protected AacFlBase aac { get; private set; }
        protected T my { get; private set; }
        protected BuildContext context { get; private set; }

        public override string QualifiedName => $"dev.angelware.ndmf-processor::{GetType().FullName}";
        public override string DisplayName => $"NdmfAsCode for {GetType().Name}";

        protected virtual ApsMergerPluginOutput Execute()
        {
            return ApsMergerPluginOutput.Regular();
        }

        protected override void Configure()
        {
            if (GetType() == typeof(ApsMergerPlugin<>)) return;

            InPhase(BuildPhase.Generating)
                .AfterPlugin((NdmfAacDBTPlugin)NdmfAacDBTPlugin.Instance)
                .Run($"Run NdmfAsCode for {GetType().Name}", ctx =>
                {
                    Debug.Log($"(self-log) Running aac plugin ({GetType().FullName}");
                    var results = new List<ApsMergerPluginOutput>();

                    var scripts = ctx.AvatarRootObject.GetComponentsInChildren(typeof(T), true);
                    foreach (var currentScript in scripts)
                    {
                        var script = (T)currentScript;
                        aac = AacV1.Create(new AacConfiguration
                        {
                            SystemName = SystemName(script, ctx),
                            AnimatorRoot = AnimatorRoot(script, ctx),
                            DefaultValueRoot = DefaultValueRoot(script, ctx),
                            AssetKey = GUID.Generate().ToString(),
                            AssetContainer = ctx.AssetContainer,
                            ContainerMode = AacConfiguration.Container.OnlyWhenPersistenceRequired,
                            DefaultsProvider = new AacDefaultsProvider(UseWriteDefaults(script, ctx))
                        });
                        my = script;
                        context = ctx;

                        Execute();
                    }

                    // Get rid of the short-lived sugar fields
                    aac = null;
                    my = null;
                    context = null;

                    var overrides = new Dictionary<string, float>();
                    foreach (var result in results)
                    {
                        foreach (var over in result.overrides)
                        {
                            if (!overrides.ContainsKey(over.Key))
                            {
                                overrides.Add(over.Key, over.Value);
                            }
                        }
                    }
                    
                    var state = ctx.GetState<InternalApsMergerPluginState>();
                    state.directBlendTreeMembers = results.SelectMany(output => output.members).ToArray();
                    state.directBlendTreeOverrides = overrides;
                }).BeforePlugin("ANGELWARE.AW_AAC.AW_MergeDbts");
        }
    }

    internal class InternalApsMergerPluginState
    {
        public ApsMergerPluginOutput.DirectBlendTreeMember[] directBlendTreeMembers;
        public Dictionary<string, float> directBlendTreeOverrides;
    }

    public struct ApsMergerPluginOutput
    {
        public DirectBlendTreeMember[] members;
        public Dictionary<string, float> overrides;

        public ApsMergerPluginOutput OverrideValue(AacFlParameter parameter, float value)
        {
            overrides.Add(parameter.Name, value);
            return this;
        }

        public ApsMergerPluginOutput OverrideValue(string parameter, float value)
        {
            overrides.Add(parameter, value);
            return this;
        }

        public static ApsMergerPluginOutput Regular()
        {
            return new ApsMergerPluginOutput
            {
                members = Array.Empty<DirectBlendTreeMember>(),
                overrides = new Dictionary<string, float>()
            };
        }

        public static ApsMergerPluginOutput DirectBlendTree(VRCAvatarDescriptor.AnimLayerType layerType, params AacFlBlendTree[] members)
        {
            return DirectBlendTree(layerType, members.Select(tree => (Motion)tree.BlendTree).ToArray());
        }

        public static ApsMergerPluginOutput DirectBlendTree(VRCAvatarDescriptor.AnimLayerType layerType, params Motion[] members)
        {
            return new ApsMergerPluginOutput
            {
                members = members
                    .Select(motion => new DirectBlendTreeMember
                    {
                        layerType = layerType,
                        member = motion
                    })
                    .ToArray(),
                overrides = new Dictionary<string, float>()
            };
        }
    
        public struct DirectBlendTreeMember
        {
            public VRCAvatarDescriptor.AnimLayerType layerType;
            public Motion member;
            
            public string parameterOptional;
        }
    }
    /*
     *  MIT License

        Copyright (c) 2023 Haï~ (@vr_hai github.com/hai-vr)

        Permission is hereby granted, free of charge, to any person obtaining a copy
        of this software and associated documentation files (the "Software"), to deal
        in the Software without restriction, including without limitation the rights
        to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
        copies of the Software, and to permit persons to whom the Software is
        furnished to do so, subject to the following conditions:

        The above copyright notice and this permission notice shall be included in all
        copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
        IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
        FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
        AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
        LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
        SOFTWARE.
     */
    
}
#endif