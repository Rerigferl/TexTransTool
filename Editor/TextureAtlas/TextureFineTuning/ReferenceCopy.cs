#if UNITY_EDITOR
using System.Collections.Generic;

namespace net.rs64.TexTransTool.TextureAtlas.FineSetting
{
    public class ReferenceCopy : IAddFineTuning
    {

        public string SourcePropertyName;
        public string TargetPropertyName;

        public ReferenceCopy(string sourcePropertyName, string targetPropertyName)
        {
            SourcePropertyName = sourcePropertyName;
            TargetPropertyName = targetPropertyName;
        }

        public void AddSetting(List<TexFineTuningTarget> propAndTextures)
        {
            var TexTarget = propAndTextures.Find(x => x.PropertyName == SourcePropertyName);
            if (TexTarget == null) return;

            var referenceCopyData = TexTarget.TuningDataList.Find(I => I is ReferenceCopyData) as ReferenceCopyData;
            if (referenceCopyData != null)
            {
                referenceCopyData.CopySouse = TargetPropertyName;
            }
            else
            {
                TexTarget.TuningDataList.Add(new ReferenceCopyData(TargetPropertyName));
            }

        }
    }

    public class ReferenceCopyData : ITuningData
    {
        public string CopySouse;

        public ReferenceCopyData(string copySouse)
        {
            CopySouse = copySouse;
        }
    }

    public class ReferenceCopyApplicant : ITuningApplicant
    {

        public int Order => 32;

        public void ApplyTuning(List<TexFineTuningTarget> texFineTuningTargets)
        {
            foreach (var texf in texFineTuningTargets)
            {
                var referenceCopyData = texf.TuningDataList.Find(I => I is ReferenceCopyData) as ReferenceCopyData;
                if (referenceCopyData == null) { continue; }

                var souse = texFineTuningTargets.Find(I => I.PropertyName == referenceCopyData.CopySouse);
                if (souse == null) { continue; }
                texf.Texture2D = souse.Texture2D;
            }
        }
    }

}
#endif
