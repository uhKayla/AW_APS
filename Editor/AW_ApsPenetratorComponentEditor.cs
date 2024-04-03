#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.SDKBase;

namespace ANGELWARE.AW_APS.Editor
{
    [CustomEditor(typeof(AW_ApsPenetratorComponent))]
    public class AW_ApsPenetratorComponentEditor : AW_BaseInspector, IEditorOnly
    {
        private SerializedProperty _rootBone;
        private SerializedProperty _tipBone;

        private SerializedProperty _oneFloatParameter;
        private SerializedProperty _smoothingMultiplierParameter;

        private SerializedProperty _parameterName;

        private SerializedProperty _wetnessSpeed;
        private SerializedProperty _wetnessAnimations;

        private SerializedProperty _skinMovementSpeed;
        private SerializedProperty _skinMovementAnimations;

        private SerializedProperty _sizeAnimations;
        private SerializedProperty _ballsSizeAnimations;

        private SerializedProperty _foreskinAnimations;
        private SerializedProperty _shaftAppearanceAnimations;
        private SerializedProperty _ballsAppearanceAnimations;

        private SerializedProperty _idleMovementAnimation;

        private SerializedProperty _erectionAnimations;

        private SerializedProperty _cumOn;
        private SerializedProperty _cumObject;
        private SerializedProperty _staticObject;
        private SerializedProperty _ocsFinishSender;

        private SerializedProperty _frotEnable;
        private SerializedProperty _frotDisable;

        protected void OnEnable()
        {
            _rootBone = serializedObject.FindProperty("rootTransform");
            _tipBone = serializedObject.FindProperty("tipTransform");
            _oneFloatParameter = serializedObject.FindProperty("oneFloatParameter");
            _smoothingMultiplierParameter = serializedObject.FindProperty("smoothingMultiplierParam");
            _parameterName = serializedObject.FindProperty("parameterName");
            _wetnessSpeed = serializedObject.FindProperty("wetnessSpeed");
            _wetnessAnimations  = serializedObject.FindProperty("wetnessAnimations");
            _skinMovementSpeed = serializedObject.FindProperty("skinMovementSpeed");
            _skinMovementAnimations = serializedObject.FindProperty("skinMovementAnimations");
            _sizeAnimations = serializedObject.FindProperty("sizeAnimations");
            _ballsSizeAnimations = serializedObject.FindProperty("ballsSizeAnimations");
            _foreskinAnimations = serializedObject.FindProperty("foreskinAnimations");
            _shaftAppearanceAnimations = serializedObject.FindProperty("shaftAppearanceAnimations");
            _ballsAppearanceAnimations = serializedObject.FindProperty("ballsAppearanceAnimations");
            _idleMovementAnimation = serializedObject.FindProperty("idleMovementAnimation");
            _erectionAnimations = serializedObject.FindProperty("erectionAnimations");
            _cumOn = serializedObject.FindProperty("enableCumEffects");
            _cumObject = serializedObject.FindProperty("cumObject");
            _staticObject = serializedObject.FindProperty("staticCumObject");
            _ocsFinishSender = serializedObject.FindProperty("ocsFinishSender");
            _frotDisable = serializedObject.FindProperty("frotDisable");
            _frotEnable = serializedObject.FindProperty("frotEnable");
        }

        protected override void SetupContent(VisualElement root)
        {
            base.SetupContent(root);

            var container = root.Q<VisualElement>("Container");

            var introText = new Label("Use this component to set up APS compatible penetrators. " +
                                      "Assign the root bone of your penetrator and the tip of your penetrator to " +
                                      "automatically calculate the length of contacts.\n\n" +
                                      "Please note that APS does not currently create penetrator shaders! Only components for animation!");
            introText.style.flexWrap = Wrap.Wrap;
            introText.style.whiteSpace = WhiteSpace.Normal;
            introText.style.paddingBottom = 5;
            container.Add(introText);

            var seperator = new VisualElement();
            seperator.style.borderBottomColor = new StyleColor(new Color(0, 0, 0, 0.5f));
            seperator.style.borderBottomWidth = 1;
            container.Add(seperator);

            var boneText = new Label("Transforms");
            boneText.style.unityTextAlign = TextAnchor.MiddleCenter;
            boneText.style.fontSize = 15;
            boneText.style.paddingTop = 5;
            boneText.style.paddingBottom = 5;
            container.Add(boneText);

            var paramName = new PropertyField(_parameterName);
            container.Add(paramName);
            var rootBone = new PropertyField(_rootBone);
            container.Add(rootBone);
            var tipBone = new PropertyField(_tipBone);
            tipBone.style.paddingBottom = 5;
            container.Add(tipBone);
            
            var aseperator = new VisualElement();
            aseperator.style.borderBottomColor = new StyleColor(new Color(0, 0, 0, 0.5f));
            aseperator.style.borderBottomWidth = 1;
            container.Add(aseperator);

            var frotEnable = new PropertyField(_frotEnable);
            container.Add(frotEnable);
            var frotDisable = new PropertyField(_frotDisable);
            frotDisable.style.paddingBottom = 5;
            container.Add(frotDisable);
            
            var jseperator = new VisualElement();
            jseperator.style.borderBottomColor = new StyleColor(new Color(0, 0, 0, 0.5f));
            jseperator.style.borderBottomWidth = 1;
            container.Add(jseperator);
            
            var animationPropText = new Label("Animation Properties");
            animationPropText.style.unityTextAlign = TextAnchor.MiddleCenter;
            animationPropText.style.fontSize = 15;
            animationPropText.style.paddingTop = 5;
            animationPropText.style.paddingBottom = 5;
            container.Add(animationPropText);

            var oneFloat = new PropertyField(_oneFloatParameter);
            container.Add(oneFloat);
            var smoothingMult = new PropertyField(_smoothingMultiplierParameter);
            container.Add(smoothingMult);
            
            var bseperator = new VisualElement();
            bseperator.style.borderBottomColor = new StyleColor(new Color(0, 0, 0, 0.5f));
            bseperator.style.borderBottomWidth = 1;
            container.Add(bseperator);
            
            var animationText = new Label("Animations");
            animationText.style.unityTextAlign = TextAnchor.MiddleCenter;
            animationText.style.fontSize = 15;
            animationText.style.paddingTop = 5;
            animationText.style.paddingBottom = 5;
            container.Add(animationText);
            
            var warningText = new Label("Warning! The following parameters only work with AAPS installed!");
            warningText.style.flexWrap = Wrap.Wrap;
            warningText.style.whiteSpace = WhiteSpace.Normal;
            warningText.style.paddingBottom = 5;
            warningText.style.paddingTop = 5;
            container.Add(warningText);
            
            var cseperator = new VisualElement();
            cseperator.style.borderBottomColor = new StyleColor(new Color(0, 0, 0, 0.5f));
            cseperator.style.borderBottomWidth = 1;
            container.Add(cseperator);

            var erectionAnims = new PropertyField(_erectionAnimations);
            container.Add(erectionAnims);

            var hseperator = new VisualElement();
            hseperator.style.borderBottomColor = new StyleColor(new Color(0, 0, 0, 0.5f));
            hseperator.style.borderBottomWidth = 1;
            container.Add(hseperator);
            
            var wetnessSpeed = new PropertyField(_wetnessSpeed);
            container.Add(wetnessSpeed);
            var wetnessAnims = new PropertyField(_wetnessAnimations);
            container.Add(wetnessAnims);
            
            var dseperator = new VisualElement();
            dseperator.style.borderBottomColor = new StyleColor(new Color(0, 0, 0, 0.5f));
            dseperator.style.borderBottomWidth = 1;
            container.Add(dseperator);

            var skinSpeed = new PropertyField(_skinMovementSpeed);
            container.Add(skinSpeed);
            var skinMovementAnims = new PropertyField(_skinMovementAnimations);
            container.Add(skinMovementAnims);
            
            var eseperator = new VisualElement();
            eseperator.style.borderBottomColor = new StyleColor(new Color(0, 0, 0, 0.5f));
            eseperator.style.borderBottomWidth = 1;
            container.Add(eseperator);
            
            var sizeAnimations = new PropertyField(_sizeAnimations);
            container.Add(sizeAnimations);
            var ballsSizeAnimations = new PropertyField(_ballsSizeAnimations);
            container.Add(ballsSizeAnimations);
            
            var fseperator = new VisualElement();
            fseperator.style.borderBottomColor = new StyleColor(new Color(0, 0, 0, 0.5f));
            fseperator.style.borderBottomWidth = 1;
            container.Add(fseperator);
            
            var foreskinAnimations = new PropertyField(_foreskinAnimations);
            container.Add(foreskinAnimations);
            var shaftAppearanceAnimations = new PropertyField(_shaftAppearanceAnimations);
            container.Add(shaftAppearanceAnimations);
            var ballsAppearanceAnimations = new PropertyField(_ballsAppearanceAnimations);
            container.Add(ballsAppearanceAnimations);
            
            var gseperator = new VisualElement();
            gseperator.style.borderBottomColor = new StyleColor(new Color(0, 0, 0, 0.5f));
            gseperator.style.borderBottomWidth = 1;
            container.Add(gseperator);
            
            var idleMovementAnimation = new PropertyField(_idleMovementAnimation);
            container.Add(idleMovementAnimation);
            
            var iseperator = new VisualElement();
            iseperator.style.borderBottomColor = new StyleColor(new Color(0, 0, 0, 0.5f));
            iseperator.style.borderBottomWidth = 1;
            container.Add(iseperator);
            
            var enableCumEffects = new PropertyField(_cumOn);
            container.Add(enableCumEffects);
            var cumObjectField = new PropertyField(_cumObject);
            container.Add(cumObjectField);
            var staticCumObject = new PropertyField(_staticObject);
            container.Add(staticCumObject);
            var ocsFinishSender = new PropertyField(_ocsFinishSender);
            container.Add(ocsFinishSender);
        }
    }
}
#endif