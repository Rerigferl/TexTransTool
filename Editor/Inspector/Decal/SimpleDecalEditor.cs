#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEditor;
using net.rs64.TexTransTool.Decal;

namespace net.rs64.TexTransTool.Editor.Decal
{

    [CustomEditor(typeof(SimpleDecal), true)]
    public class SimpleDecalEditor : UnityEditor.Editor
    {
        bool FoldoutOption;
        public override void OnInspectorGUI()
        {
            var This_S_Object = serializedObject;
            var ThisObject = target as SimpleDecal;

            EditorGUI.BeginDisabledGroup(PreviewContext.IsPreviewing(ThisObject));

            AbstractDecalEditor.DrawerDecalEditor(This_S_Object);

            if (targets.Length == 1)
            {
                var tf_S_Obg = new SerializedObject(ThisObject.transform);
                var decalTexture = ThisObject.DecalTexture;
                DrawerScale(This_S_Object, tf_S_Obg, decalTexture);
                tf_S_Obg.ApplyModifiedProperties();
            }

            EditorGUILayout.LabelField("CullingSettings".GetLocalize(), EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;

            var s_PolygonCulling = This_S_Object.FindProperty("PolygonCulling");
            EditorGUILayout.PropertyField(s_PolygonCulling, s_PolygonCulling.name.GetLC());

            var s_SideCulling = This_S_Object.FindProperty("SideCulling");
            EditorGUILayout.PropertyField(s_SideCulling, s_SideCulling.name.GetLC());

            var s_IslandCulling = This_S_Object.FindProperty("IslandCulling");
            EditorGUILayout.PropertyField(s_IslandCulling, s_IslandCulling.name.GetLC());
            if (s_IslandCulling.boolValue)
            {
                var s_IslandSelectorPos = This_S_Object.FindProperty("IslandSelectorPos");
                EditorGUI.indentLevel += 1;
                EditorGUILayout.LabelField(s_IslandSelectorPos.name.GetLocalize());
                EditorGUI.indentLevel += 1;
                var s_IslandSelectorPosX = s_IslandSelectorPos.FindPropertyRelative("x");
                var s_IslandSelectorPosY = s_IslandSelectorPos.FindPropertyRelative("y");
                EditorGUILayout.Slider(s_IslandSelectorPosX, 0, 1, new GUIContent("x"));
                EditorGUILayout.Slider(s_IslandSelectorPosY, 0, 1, new GUIContent("y"));
                EditorGUI.indentLevel -= 1;
                var s_IslandSelectorRange = This_S_Object.FindProperty("IslandSelectorRange");
                EditorGUILayout.Slider(s_IslandSelectorRange, 0, 1, s_IslandSelectorRange.name.GetLC());
                EditorGUI.indentLevel -= 1;
            }
            EditorGUI.indentLevel -= 1;

            AbstractDecalEditor.DrawerAdvancedOption(This_S_Object);

            EditorGUI.EndDisabledGroup();
            AbstractDecalEditor.DrawerRealTimePreviewEditor(ThisObject);
            EditorGUI.BeginDisabledGroup(RealTimePreviewManager.instance.RealTimePreviews.ContainsKey(ThisObject));
            PreviewContext.instance.DrawApplyAndRevert(ThisObject);
            EditorGUI.EndDisabledGroup();

            This_S_Object.ApplyModifiedProperties();
        }

        public static void DrawerScale(SerializedObject This_S_Object, SerializedObject tf_S_Obg, Texture2D decalTexture)
        {
            EditorGUILayout.LabelField("ScaleSettings".GetLocalize(), EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;

            var s_localScale = tf_S_Obg.FindProperty("m_LocalScale");
            var s_FixedAspect = This_S_Object.FindProperty("FixedAspect");

            System.Action<float> editCollBack = (value) => { s_localScale.FindPropertyRelative("y").floatValue = value * ((float)decalTexture.height / (float)decalTexture.width); };
            if (s_FixedAspect.boolValue)
            {
                TextureTransformerEditor.DrawerPropertyFloat(
                    s_localScale.FindPropertyRelative("x"),
                    s_localScale.displayName.GetLC(),
                    editCollBack
                );
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scale".GetLocalize(), GUILayout.Width(60));
                EditorGUILayout.LabelField("x", GUILayout.Width(30));
                EditorGUILayout.PropertyField(s_localScale.FindPropertyRelative("x"), GUIContent.none);
                EditorGUILayout.LabelField("y", GUILayout.Width(30));
                EditorGUILayout.PropertyField(s_localScale.FindPropertyRelative("y"), GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }

            TextureTransformerEditor.DrawerPropertyBool(s_FixedAspect, s_FixedAspect.displayName.GetLC(), (Value) => { if (Value) { editCollBack.Invoke(s_localScale.FindPropertyRelative("x").floatValue); } });

            EditorGUILayout.PropertyField(s_localScale.FindPropertyRelative("z"), "MaxDistance".GetLC());

            EditorGUI.indentLevel -= 1;
        }

        public static void DrawerSummary(SimpleDecal target)
        {
            var s_obj = new SerializedObject(target);

            var s_TargetRenderers = s_obj.FindProperty("TargetRenderers");
            TextureTransformerEditor.DrawerTargetRenderersSummary(s_TargetRenderers);

            var s_DecalTexture = s_obj.FindProperty("DecalTexture");
            TextureTransformerEditor.DrawerObjectReference<Texture2D>(s_DecalTexture, s_DecalTexture.name.GetLocalize());

            s_obj.ApplyModifiedProperties();
        }


    }


}
#endif
