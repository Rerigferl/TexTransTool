using UnityEditor;
using net.rs64.TexTransTool.Editor;
using net.rs64.TexTransTool;
using UnityEngine.UIElements;
using net.rs64.TexTransTool.Build;
using System.Collections.Generic;

namespace net.rs64.TexTransTool.Editor
{
    [CustomEditor(typeof(PreviewGroup))]
    internal class PreviewGroupEditor : TexTransGroupEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            LoadStyle();

            var rootVE = new VisualElement();
            var previewButton = new IMGUIContainer(() => { PreviewContext.instance.DrawApplyAndRevert(target as TexTransGroup); });

            rootVE.hierarchy.Add(previewButton);
            rootVE.styleSheets.Add(s_style);

            var previewGroup = target as PreviewGroup;
            var phase = AvatarBuildUtils.FindAtPhaseAll(previewGroup.gameObject);

            var label = new Label(TexTransPhase.BeforeUVModification.ToString());
            label.style.fontSize = 16f;
            rootVE.hierarchy.Add(label);
            CreateGroupElements(rootVE, phase[TexTransPhase.BeforeUVModification], true);

            label = new Label(TexTransPhase.UVModification.ToString());
            label.style.fontSize = 16f;
            rootVE.hierarchy.Add(label);
            CreateGroupElements(rootVE, phase[TexTransPhase.UVModification], true);


            label = new Label(TexTransPhase.AfterUVModification.ToString());
            label.style.fontSize = 16f;
            rootVE.hierarchy.Add(label);
            CreateGroupElements(rootVE, phase[TexTransPhase.AfterUVModification], true);

            label = new Label(TexTransPhase.UnDefined.ToString());
            label.style.fontSize = 16f;
            rootVE.hierarchy.Add(label);
            CreateGroupElements(rootVE, phase[TexTransPhase.UnDefined], true);

            return rootVE;
        }

    }
}
