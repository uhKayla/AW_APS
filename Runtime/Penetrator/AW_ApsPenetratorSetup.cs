#if UNITY_EDITOR
using System.Collections.Generic;
using ANGELWARE.AW_APS;
using ANGELWARE.AW_OCS;
using nadena.dev.ndmf;
using UnityEngine;

[assembly: ExportsPlugin(typeof(AW_ApsPenetratorSetup))]

namespace ANGELWARE.AW_APS
{
    public class AW_ApsPenetratorSetup : Plugin<AW_ApsPenetratorSetup>
    {
        private AW_ContactReceiver _receiver;
        private AW_ContactSender _sender;
        private AW_ApsPenetratorComponent _apsPenComponent;

        public override string QualifiedName => "AW.APS.Penetrator";

        protected override void Configure()
        {
            InPhase(BuildPhase.Transforming).Run("AW_APS Build Holes", ctx =>
            {
                _apsPenComponent = Object.FindObjectOfType<AW_ApsPenetratorComponent>();
                
                if (_apsPenComponent == null) return;
                var root = _apsPenComponent.transform;
                
                var distance = Vector3.Distance(_apsPenComponent.rootTransform.transform.position, _apsPenComponent.tipTransform.transform.position);
                
                // Create Contact
                var contact = new GameObject("APS_Receiver");
                contact.transform.SetParent(_apsPenComponent.rootTransform);
                
                var contactReceiverClass = new AW_ContactReceiver();
                contactReceiverClass.CreateContactReceiver(contact, _apsPenComponent.rootTransform, 0, distance, null,
                    new Vector3(0, 0, 0), new Vector3(0, 0, 0), new List<string> { "TPS_Orf_Root" }, true, true, false, 2,
                    $"APS/{_apsPenComponent.parameterName}/Depth");
                
                // Create Contact
                var pContact = new GameObject("APS_PenetratingReceiver");
                pContact.transform.SetParent(_apsPenComponent.rootTransform);
                
                contactReceiverClass.CreateContactReceiver(pContact, _apsPenComponent.rootTransform, 0, distance, null,
                    new Vector3(0, 0, 0), new Vector3(0, 0, 0), new List<string> { "TPS_Orf_Root" }, true, true, false, 0,
                    $"APS/Internal/IsPenetrating");
                
                // // Create Contact
                // var dContact = new GameObject("APS_PenetratingDepthReceiver");
                // dContact.transform.SetParent(_apsPenComponent.rootTransform);
                //
                // contactReceiverClass.CreateContactReceiver(dContact, _apsPenComponent.rootTransform, 0, distance, null,
                //     new Vector3(0, 0, 0), new Vector3(0, 0, 0), new List<string> { "TPS_Orf_Root" }, true, true, false, 2,
                //     $"APS/{_apsPenComponent.parameterName}/Depth");
            });
        }
    }
}
#endif