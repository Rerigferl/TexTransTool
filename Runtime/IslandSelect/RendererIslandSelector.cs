using System.Collections;
using System.Collections.Generic;
using System.Linq;
using net.rs64.TexTransTool.UVIsland;
using UnityEngine;

namespace net.rs64.TexTransTool.IslandSelector
{
    [AddComponentMenu(TexTransBehavior.TTTName + "/" + MenuPath)]
    public class RendererIslandSelector : AbstractIslandSelector
    {
        internal const string ComponentName = "TTT RendererIslandSelector";
        internal const string MenuPath = FoldoutName + "/" + ComponentName;
        internal override void LookAtCalling(ILookingObject looker) { looker.LookAt(this); }
        public List<Renderer> RendererList;
        internal override BitArray IslandSelect(Island[] islands, IslandDescription[] islandDescription)
        {
            var bitArray = new BitArray(islands.Length);
            if (RendererList.Count == 0)
                return bitArray;
            var hash = RendererList.ToHashSet();

            for (int i = 0; i < islands.Length; i += 1)
            {
                var renderer = islandDescription[i].Renderer;
                bool flag = hash.Contains(renderer);
#if UNITY_EDITOR && NDMF_1_6_1_OR_NEWER

                if (!flag)
                {
                    var original = nadena.dev.ndmf.preview.NDMFPreview.GetOriginalObjectForProxy(renderer.gameObject);
                    Debug.LogError($"{renderer.gameObject} => {original}");
                    if (original?.TryGetComponent(out renderer) == true)
                    {
                        flag = hash.Contains(renderer);
                    }
                }
#endif

                bitArray[i] = flag;
            }

            return bitArray;
        }
        internal override void OnDrawGizmosSelected() { }
    }
}
