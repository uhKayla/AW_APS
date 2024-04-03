// #if UNITY_EDITOR
// using System.Collections.Generic;
// using ANGELWARE.AW_APS;
// using ANGELWARE.AW_OCS;
// using ANGELWARE.AW_AAC;
// using ANGELWARE.AW_AAPS;
// using AnimatorAsCode.V1;
// using AnimatorAsCode.V1.ModularAvatar;
// using AnimatorAsCode.V1.NDMFProcessor;
// using AnimatorAsCode.V1.VRC;
// using nadena.dev.ndmf;
// using UnityEngine;
// using VRC.SDK3.Avatars.Components;
// using VRC.SDKBase;
//
// [assembly: ExportsPlugin(typeof(AW_ApsPenetratorPlugin))]
//
// namespace ANGELWARE.AW_AAPS
// {
//     [AddComponentMenu("ANGELWARE/APS/AW_Penetrator")]
//
//     public class AW_ApsPenetrator : MonoBehaviour, IEditorOnly
//     {
//         public Transform rootTransform;
//         public Transform tipTransform;
//         public string oneFloatParameter = "Internal/Float";
//         public string smoothingMultiplierParam = "Internal/MultipliedSmooth";
//         public string speedMultiplierParam = "Internal/APS/WetnessSpeed";
//         public string parameterName = "Penetrator";
//         public float speedMultiplier = 1f;
//         // public List<Parameter> paramList = new();
//         
//         public AW_ApsSerializableAnimations[] wetnessAnimations;
//     }
//
//     public class AW_ApsPenetratorPlugin : AacPlugin<AW_ApsPenetrator>
//     {
//         protected override AacPluginOutput Execute()
//         {
//             #region Setup
//             var ctrl = aac.NewAnimatorController();
//             var fx = ctrl.NewLayer();
//             var maAc = MaAc.Create(my.gameObject);
//             
//             var oneFloatParameter = aac.NoAnimator().FloatParameter(my.oneFloatParameter);
//             var dbtMaster = aac.NewBlendTree().Direct();
//
//             fx.NewState("(WD On) Master").WithAnimation(dbtMaster).WithWriteDefaultsSetTo(true);
//             #endregion
//
//             #region Calculate Size
//             //Calculate Size
//             var distance = Vector3.Distance(my.rootTransform.transform.position, my.tipTransform.transform.position);
//
//             // Create Contact
//             var contact = new GameObject("APS_Receiver");
//             contact.transform.SetParent(my.rootTransform);
//             
//             var contactReceiverClass = new AW_ContactReceiver();
//             contactReceiverClass.CreateContactReceiver(contact, my.rootTransform, 0, distance, null,
//                 new Vector3(0, 0, 0), new Vector3(0, 0, 0), new List<string> { "TPS_Orf_Root" }, true, true, false, 2,
//                 $"APS/NSFW/{my.parameterName}");
//             
//             // Create Contact
//             var pContact = new GameObject("APS_PenetratingReceiver");
//             pContact.transform.SetParent(my.rootTransform);
//             
//             contactReceiverClass.CreateContactReceiver(pContact, my.rootTransform, 0, distance, null,
//                 new Vector3(0, 0, 0), new Vector3(0, 0, 0), new List<string> { "TPS_Orf_Root" }, true, true, false, 0,
//                 $"APS/Internal/IsPenetrating");
//             #endregion
//
//             #region Parameters
//
//             var targetP = fx.FloatParameter($"APS/NSFW/{my.parameterName}");
//             var speedMult = fx.FloatParameter(my.speedMultiplierParam);
//             fx.OverrideValue(speedMult, my.speedMultiplier);
//             fx.FloatParameter("APS/Multiplied/WetnessSpeed");
//             var wetnessSpeed = fx.FloatParameter("Internal/APS/WetnessSpeed");
//             fx.OverrideValue(wetnessSpeed, my.speedMultiplier);
//             fx.FloatParameter($"APS/NSFW/{my.parameterName}");
//             var smoothedP = fx.FloatParameter($"APS/NSFW/Smooth/{my.parameterName}");
//
//             var speedMultiplierParameter = aac.NoAnimator().FloatParameter(my.speedMultiplierParam);
//             var speedMultipliedOutput = aac.NoAnimator().FloatParameter("APS/Multiplied/WetnessSpeed");
//             #endregion
//             
//             #region Multiplier
//             
//             // Clip with our output value at 1.0f
//             var multiplierClip = aac.NewClip()
//                 .Animating(
//                     clip =>
//                     {
//                         clip.AnimatesAnimator(speedMultipliedOutput).WithOneFrame(3.0f);
//                     });
//             
//             // Multiplier with our speed parameter
//             var multiplier = aac.NewBlendTree().Simple1D(speedMultiplierParameter)
//                 .WithAnimation(multiplierClip, 1.0f);
//             
//             // Multiply that value with our frametime multiplier
//             var multipliedSpeed = aac.NewBlendTree()
//                 .Simple1D(aac.NoAnimator().FloatParameter(my.smoothingMultiplierParam)).WithAnimation(multiplier, 1.0f);
//             
//             // Add those trees to our master
//             dbtMaster.WithAnimation(multipliedSpeed, oneFloatParameter);
//             #endregion
//             
//             // #region Animation Tree
//             // var menuParam = aac.NoAnimator().FloatParameter($"APS/NSFW/{my.parameterName}");
//             // var smoothedParam = aac.NoAnimator().FloatParameter($"APS/NSFW/Smooth/{my.parameterName}");
//             // var multSmooth = aac.NoAnimator().FloatParameter(my.speedMultiplierParam);
//             // var oneFloat = aac.NoAnimator().FloatParameter(my.oneFloatParameter);
//             //
//             // var animation1d = aac.NewBlendTree().Simple1D(smoothedParam);
//             //
//             // // if (my.anims.Length != my.floatTrigger.Length)
//             // // {
//             // //     Debug.LogError("The lengths of AnimationClips and floatTriggers arrays do not match.");
//             // //     return AacPluginOutput.Regular();
//             // // }
//             //
//             // // index all animations and trigger vals and add them to 1d tree.
//             // // for (var i = 0; i < my.anims.Length; i++)
//             // // {
//             // //     var animation = my.anims[i];
//             // //     var triggerValue = my.floatTrigger[i];
//             // //     
//             // //     animation1d.WithAnimation(animation, triggerValue);
//             // // }
//             //
//             // foreach (var serializedAnimation in my.wetnessAnimations)
//             // {
//             //     animation1d.WithAnimation(serializedAnimation.animation, serializedAnimation.trigger);
//             // }
//             //
//             // var aap0 = aac.NewClip().NonLooping().Animating(clip =>
//             //         {
//             //             clip.AnimatesAnimator(smoothedParam).WithOneFrame(0.0f);
//             //         }
//             //     );
//             //
//             // var aap1 = aac.NewClip().NonLooping().Animating(clip =>
//             //     {
//             //         clip.AnimatesAnimator(smoothedParam).WithOneFrame(1.0f);
//             //     }
//             // );
//             //
//             // var smoothed1d = aac.NewBlendTree()
//             //     .Simple1D(smoothedParam)
//             //     .WithAnimation(aap0, 0.0f)
//             //     .WithAnimation(aap1, 1.0f);
//             //
//             // var menu1d = aac.NewBlendTree()
//             //     .Simple1D(menuParam)
//             //     .WithAnimation(aap0, 0.0f)
//             //     .WithAnimation(aap1, 1.0f);
//             //
//             // var smoothing1d = aac.NewBlendTree()
//             //     .Simple1D(multSmooth)
//             //     .WithAnimation(smoothed1d, 0.0f)
//             //     .WithAnimation(menu1d, 1.0f);
//             //
//             // var dbt = aac.NewBlendTree().Direct()
//             //     .WithAnimation(smoothing1d, oneFloat)
//             //     .WithAnimation(animation1d, oneFloat);
//             //
//             // dbtMaster.WithAnimation(dbt, oneFloat);
//             // #endregion
//             
//             #region Linear Animation Tree
//             // var wetnessTree = ApsSmoothTree("Penetration", my.wetnessAnimations);
//             //
//             // dbtMaster.WithAnimation(wetnessTree, oneFloatParameter);
//             #endregion
//             
//             // ArousalController(my.parameterName, ctrl, 0.99f, 0.01f, "NSFW/Input/Arousal", "OCS/NSFW/Touch", 5f, 0f, 1f, 0.01f, 0.005f);
//
//             
//             // AW_DirectTreeManager.AddDbtToList(dbtMaster, my.paramList);
//             maAc.NewMergeAnimator(ctrl, VRCAvatarDescriptor.AnimLayerType.FX);
//             
//             return AacPluginOutput.Regular();
//         }
//         
//         private void WetnessController(string layerName, AacFlController ctrl, float maxValue, float minValue, string inputParameter, string desiredParameter, float waitSeconds, float activationThreshold, float deactivationThreshold, float increaseBy, float decreaseBy)
//         {
//             var nullObject = new GameObject("NullObject");
//             var arousalLayer = ctrl.NewLayer(layerName);
//             var nullAnim = aac.NewClip().Toggling(nullObject, true);
//
//             var tenSeconds = aac.NewClip("10s").Animating(clip =>
//             {
//                 clip.Animates(nullObject).WithFixedSeconds(waitSeconds, 1f);
//             });
//             
//             var twoSeconds = aac.NewClip("2s").Animating(clip =>
//             {
//                 clip.Animates(nullObject).WithFixedSeconds(2f, 1f);
//             });
//             
//             var halfSecond = aac.NewClip("2s").Animating(clip =>
//             {
//                 clip.Animates(nullObject).WithFixedSeconds(0.5f, 1f);
//             });
//
//             var pArousal = arousalLayer.FloatParameter(inputParameter);
//             var pOcsTouch = arousalLayer.FloatParameter(desiredParameter);
//
//             // Idle State
//             var idleState = arousalLayer.NewState("Idle").WithAnimation(nullAnim);
//             
//             // Init Touch State (From any, player just touched)
//             var initTouchState = arousalLayer.NewState("InitTouch").WithAnimation(nullAnim);
//             
//             // Add State (From Init, player still touching)
//             var addState = arousalLayer.NewState("AddState").WithAnimation(halfSecond);
//             addState.DrivingIncreases(pArousal, increaseBy);
//             
//             // 10s timer waiting state
//             var waitingState = arousalLayer.NewState("Wait").WithAnimation(tenSeconds);
//
//             // Subtraction Buffer State
//             var subtractWaitState = arousalLayer.NewState("SubtractWait").WithAnimation(twoSeconds);
//
//             // Subtraction State
//             var subtractState = arousalLayer.NewState("Subtract").WithAnimation(nullAnim);
//             subtractState.DrivingDecreases(pArousal, decreaseBy);
//             
//             // idleState.TransitionsFromEntry(); (Automatic)
//
//             initTouchState.TransitionsFromAny().WithNoTransitionToSelf().Automatically()
//                 .When(pOcsTouch.IsGreaterThan(activationThreshold));
//             initTouchState.TransitionsTo(addState).When(pOcsTouch.IsGreaterThan(activationThreshold))
//                 .And(pArousal.IsLessThan(maxValue));
//             initTouchState.TransitionsTo(idleState).When(pOcsTouch.IsLessThan(deactivationThreshold))
//                 .And(pArousal.IsLessThan(minValue));
//             initTouchState.TransitionsTo(waitingState).When(pOcsTouch.IsLessThan(deactivationThreshold))
//                 .And(pArousal.IsGreaterThan(minValue));
//             
//             addState.TransitionsTo(initTouchState).Automatically().WithTransitionDurationSeconds(1f);
//             
//             waitingState.TransitionsTo(addState).When(pOcsTouch.IsGreaterThan(activationThreshold));
//             waitingState.TransitionsTo(idleState).When(pArousal.IsLessThan(minValue));
//             waitingState.TransitionsTo(subtractWaitState).Automatically().AfterAnimationFinishes();
//
//             subtractWaitState.TransitionsTo(idleState).When(pArousal.IsLessThan(minValue));
//             subtractWaitState.TransitionsTo(addState).When(pOcsTouch.IsGreaterThan(activationThreshold));
//             subtractWaitState.TransitionsTo(subtractState).AfterAnimationFinishes();
//             subtractState.TransitionsTo(subtractWaitState).AfterAnimationFinishes();
//         }
//     }
//     
//     
//
// }
// #endif