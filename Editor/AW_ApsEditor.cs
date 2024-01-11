// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEngine;
// using VRC.SDKBase;
//
// namespace ANGELWARE.AW_AAC.Editor
// {
//     [AddComponentMenu("ANGELWARE/AAC/AW_APS")]
//     [CustomEditor(typeof(AW_Aps))]
//     public class AW_ApsEditor : UnityEditor.Editor, IEditorOnly
//     {
//         private Texture2D _bannerImage;
//         private SerializedProperty _defaultRecv;
//         private SerializedProperty _defaultSend;
//         private SerializedProperty _defaultTouch;
//         private SerializedProperty _defaultAnims;
//         private bool _isBannerImageNotNull;
//
//         private void OnEnable()
//         {
//             _isBannerImageNotNull = _bannerImage != null;
//             var imageGUIDtoPath = AssetDatabase.GUIDToAssetPath("6c55de3c2949cae4aa09e9a45917eb8f");
//             _bannerImage = AssetDatabase.LoadAssetAtPath<Texture2D>(imageGUIDtoPath);
//             
//             _defaultAnims = serializedObject.FindProperty("defaultAnimationsNeeded");
//             _defaultTouch = serializedObject.FindProperty("defaultTouchNeeded");
//         }
//
//         public override void OnInspectorGUI()
//         {
//             serializedObject.Update();
//
//             if (_isBannerImageNotNull)
//             {
//                 var staticHeight = 15f; 
//                 var imageWidth = staticHeight * _bannerImage.width / _bannerImage.height;
//                 float xOffset = (EditorGUIUtility.currentViewWidth - imageWidth) / 2;
//                 Rect imageRect = new Rect(xOffset, 10, imageWidth, staticHeight);
//             
//                 GUI.DrawTexture(imageRect, _bannerImage);
//                 GUILayout.Space(staticHeight + 25);
//             }
//             
//             EditorGUILayout.HelpBox("This is my custom penetration system. It is compatible with SPS, DPS, and TPS. " +
//                                     "I have written this to programatically create a more optimized derivitive of SPS. " +
//                                     "By default, we do not include most of the contact receivers / senders. However you " +
//                                     "may enable them if required. To edit assets you must use VRCFury at this time, as we " +
//                                     "do not have a native editor.",
//                 MessageType.Info, true);
//
//             EditorGUILayout.PropertyField(_defaultAnims);
//             EditorGUILayout.PropertyField(_defaultTouch);
//             
//             serializedObject.ApplyModifiedProperties();
//             
//             
//         }
//     }
// }
//
// #endif