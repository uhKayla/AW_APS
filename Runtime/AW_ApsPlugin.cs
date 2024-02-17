#if UNITY_EDITOR
using System.Collections.Generic;
using ANGELWARE.AW_APS;
using ANGELWARE.AW_OCS;
using nadena.dev.ndmf;
using UnityEngine;
using Object = UnityEngine.Object;

[assembly: ExportsPlugin(typeof(AW_ApsPlugin))]

namespace ANGELWARE.AW_APS
{
    public class AW_ApsPlugin : Plugin<AW_ApsPlugin>
    {
        private AW_ContactReceiver _receiver;
        private AW_ContactSender _sender;
        private AW_Aps _apsComponent;
        
        public override string QualifiedName => "AW.APS";

        protected override void Configure()
        {
            InPhase(BuildPhase.Transforming).Run("AW_APS Build Holes", ctx =>
            {
                _apsComponent = Object.FindObjectOfType<AW_Aps>();
                if (_apsComponent == null) return;
                var root = _apsComponent.transform;
                var markers = root.GetComponentsInChildren<AW_ApsHoleMarker>();

                foreach (var marker in markers)
                {
                    var tag = marker.tag;
                    var transform = marker.transform;
                    var rootTransform = marker.root;
                    if (rootTransform == null)
                        rootTransform = marker.transform;
                    
                    transform.SetParent(rootTransform, true);

                    var senders = CreateSenders(marker);
                    senders.transform.SetParent(transform, false);

                    var haptics = CreateSpsHaptics(marker);
                    haptics.transform.SetParent(transform, false);

                    var autoMode = CreateAutoMode(marker);
                    autoMode.transform.SetParent(transform, false);

                    var lights = CreateLights(marker);
                    lights.transform.SetParent(transform, false);

                    var animations = CreateApsAnimators(marker);
                    animations.transform.SetParent(transform, false);
                }
            });
        }

        private GameObject CreateSenders(AW_ApsHoleMarker marker)
        {
            _sender = new AW_ContactSender();

            var sendersObject = new GameObject("Senders");

            var rootList = new List<string> { "TPS_Orf_Root", "SPSLL_Socket_Root" };

            rootList.Add(!marker.ring ? "SPSLL_Socket_Hole" : "SPSLL_Socket_Ring");

            if (marker.notOnHips)
            {
                rootList.Add("TPS_Orf_Root_SelfNotOnHips");
                rootList.Add("SPSLL_Root_SelfNotOnHips");
                rootList.Add(!marker.ring ? "SPSLL_Socket_Hole_SelfNotOnHips" : "SPSLL_Socket_Ring_SelfNotOnHips");
            }

            var rootObj = new GameObject("Root");
            rootObj.transform.SetParent(sendersObject.transform);
            _sender.CreateContactSender(rootObj, rootObj.transform, 0, 0.001f,
                Vector3.zero, Vector3.zero, rootList
            );

            var frontList = new List<string> { "TPS_Orf_Norm", "SPSLL_Socket_Front" };
            if (marker.notOnHips)
            {
                rootList.Add("TPS_Orf_Norm_SelfNotOnHips");
                frontList.Add("SPSLL_Root_SelfNotOnHips");
            }

            var frontObj = new GameObject("Front");
            frontObj.transform.SetParent(sendersObject.transform);
            _sender.CreateContactSender(frontObj, frontObj.transform, 0, 0.001f,
                new Vector3(0, 0, 0.01f), Vector3.zero, frontList
            );

            return sendersObject;
        }

        private GameObject CreateSpsHaptics(AW_ApsHoleMarker holeMarker)
        {
            if (holeMarker.haptics)
            {
                _receiver = new AW_ContactReceiver();

                var haptics = new GameObject("Haptics");

                if (holeMarker.selfInteract)
                {
                    var penSelf = new GameObject("PenSelfRoot");
                    penSelf.transform.SetParent(haptics.transform);
                    _receiver.CreateContactReceiver(penSelf, penSelf.transform, 0, 1f, 0, Vector3.zero,
                        Vector3.zero, new List<string> { "TPS_Pen_Root_SelfNotOnHips" }, true,
                        false, true, 2, $"OGB/Orf/{holeMarker.tag}/PenSelfNewRoot");

                    var penSelfTip = new GameObject("PenSelfTip");
                    penSelfTip.transform.SetParent(haptics.transform);
                    _receiver.CreateContactReceiver(penSelfTip, penSelfTip.transform, 0, 1f, 0, Vector3.zero,
                        Vector3.zero, new List<string> { "TPS_Penetrating_SelfNotOnHips" }, true,
                        false, true, 2, $"OGB/Orf/{holeMarker.tag}/PenSelfNewTip");
                }

                var penOthers = new GameObject("PenOthersRoot");
                penOthers.transform.SetParent(haptics.transform);
                _receiver.CreateContactReceiver(penOthers, penOthers.transform, 0, 1f, 0, Vector3.zero,
                    Vector3.zero, new List<string> { "TPS_Penetrating_SelfNotOnHips" }, false,
                    true, true, 2, $"OGB/Orf/{holeMarker.tag}/PenOthersNewRoot");

                var penOthersTip = new GameObject("PenOthersTip");
                penOthersTip.transform.SetParent(haptics.transform);
                _receiver.CreateContactReceiver(penOthersTip, penOthersTip.transform, 0, 1f, 0, Vector3.zero,
                    Vector3.zero, new List<string> { "TPS_Penetrating_SelfNotOnHips" }, false,
                    true, true, 2, $"OGB/Orf/{holeMarker.tag}/PenOthersNewTip");

                return haptics;
            }

            return new GameObject("Haptics");
        }

        private GameObject CreateAutoMode(AW_ApsHoleMarker holeMarker)
        {
            if (holeMarker.autoMode)
            {
                _receiver = new AW_ContactReceiver();

                var receiver = new GameObject("Auto-Distance");
                // receiver.transform.SetParent(auto.transform, false);
                _receiver.CreateContactReceiver(receiver, receiver.transform, 0, 0.3f, 0, Vector3.zero,
                    Vector3.zero, new List<string> { "TPS_Penetrating" }, false,
                    true, false, 2, $"{holeMarker.tag}/AutoDistance");

                return receiver;
            }

            return new GameObject("Haptics");
        }

        private GameObject CreateLights(AW_ApsHoleMarker holeMarker)
        {
            var lightsRoot = new GameObject("Lights");

            if (!holeMarker.ring)
            {
                var rootLight = new GameObject("Root");
                rootLight.transform.SetParent(lightsRoot.transform);
                var light = rootLight.AddComponent<Light>();
                light.type = LightType.Point;
                light.range = 0.4102f;
                light.color = Color.black;
                light.lightmapBakeType = LightmapBakeType.Realtime;
                light.intensity = 1f;
                light.shadows = LightShadows.None;
                light.renderMode = LightRenderMode.ForceVertex;
            }
            else
            {
                var rootLight = new GameObject("Root");
                rootLight.transform.SetParent(lightsRoot.transform);
                var light = rootLight.AddComponent<Light>();
                light.type = LightType.Point;
                light.range = 0.4202f;
                light.color = Color.black;
                light.lightmapBakeType = LightmapBakeType.Realtime;
                light.intensity = 1f;
                light.shadows = LightShadows.None;
                light.renderMode = LightRenderMode.ForceVertex;
            }

            var frontLight = new GameObject("Front");
            frontLight.transform.localPosition = new Vector3(0, 0, 0.01f);
            frontLight.transform.SetParent(lightsRoot.transform);
            var lightFront = frontLight.AddComponent<Light>();
            lightFront.type = LightType.Point;
            lightFront.range = 0.4502f;
            lightFront.color = Color.black;
            lightFront.lightmapBakeType = LightmapBakeType.Realtime;
            lightFront.intensity = 1f;
            lightFront.shadows = LightShadows.None;
            lightFront.renderMode = LightRenderMode.ForceVertex;

            return lightsRoot;
        }

        private GameObject CreateApsAnimators(AW_ApsHoleMarker holeMarker)
        {
            _receiver = new AW_ContactReceiver();

            var auto = new GameObject("Animations");

            var receiver = new GameObject("AnimMax");
            receiver.transform.SetParent(auto.transform);

            _receiver.CreateContactReceiver(receiver, receiver.transform, 0, holeMarker.depth, 0,
                new Vector3(0, 0, -holeMarker.depth),
                Vector3.zero, new List<string> { "TPS_Pen_Penetrating" }, true,
                true, false, 2, $"NSFW/Input/{holeMarker.tag}/Max");

            var receiverEntrance = new GameObject("AnimEntrance");
            receiverEntrance.transform.SetParent(auto.transform);

            _receiver.CreateContactReceiver(receiverEntrance, receiverEntrance.transform, 0, 0.02f, 0, Vector3.zero,
                Vector3.zero, new List<string> { "TPS_Pen_Penetrating" }, true,
                true, false, 2, $"NSFW/Input/{holeMarker.tag}/Entrance");
            
            var receiverEntranceNormal = new GameObject("AnimNormal");
            receiverEntranceNormal.transform.SetParent(auto.transform);

            _receiver.CreateContactReceiver(receiverEntranceNormal, receiverEntranceNormal.transform, 0, 0.02f, 0, Vector3.zero,
                Vector3.zero, new List<string> { "TPS_Pen_Penetrating" }, true,
                true, false, 2, $"NSFW/Input/Normal");

            return auto;
        }
    }
}
#endif