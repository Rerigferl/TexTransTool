#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using Rs64.TexTransTool.Decal.Cylindrical;

namespace Rs64.TexTransTool.Decal
{
    [AddComponentMenu("TexTransTool/CylindricalDecal")]
    public class CylindricalDecal : AbstractDecal<CCSSpace>
    {
        public CylindricalCoordinatesSystem cylindricalCoordinatesSystem;
        public bool FixedAspect = true;
        public Vector2 Scale = Vector2.one;
        public bool SideChek = true;
        public float OutOfRangeOffset = 1f;
        public float FarCulling = 1f;
        public float NierCullingOffSet = 1f;

        public override CCSSpace GetSpaseConverter => new CCSSpace(cylindricalCoordinatesSystem, GetQuad());
        public override DecalUtil.ITraianglesFilter<CCSSpace> GetTraiangleFilter => new CCSFilter(GetFilters());

        private List<TrainagelFilterUtility.ITraiangleFiltaring<CCSSpace>> GetFilters()
        {
            var Filters = new List<TrainagelFilterUtility.ITraiangleFiltaring<CCSSpace>>();



            Filters.Add(new CCSFilter.BorderOnPorygonStruct());
            Filters.Add(new CCSFilter.OutOfPorigonStruct(PolygonCaling.Edge, OutOfRangeOffset, false));


            var ThisCCSZ = cylindricalCoordinatesSystem.GetCCSPoint(transform.position).z;

            Filters.Add(new CCSFilter.FarStruct(Mathf.Max(ThisCCSZ - FarCulling, 0f), false));
            Filters.Add(new CCSFilter.NeraStruct(NierCullingOffSet + ThisCCSZ, false));
            if (SideChek) Filters.Add(new CCSFilter.SideStruct());

            return Filters;
        }

        public static readonly Vector3[] LocalQuad = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
            new Vector3(-0.5f, 0.5f, 0),
            new Vector3(0.5f, 0.5f, 0),
        };


        public List<Vector3> GetQuad()
        {
            var Matrix = transform.localToWorldMatrix;
            var WorldSpaseQuad = new List<Vector3>(4);
            foreach (var i in LocalQuad)
            {
                WorldSpaseQuad.Add(Matrix.MultiplyPoint(i));
            }
            return WorldSpaseQuad;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            var Matrix = Matrix4x4.identity;
            Gizmos.matrix = Matrix;

            var CenterPos = Vector3.zero;

            var Quad = GetQuad();

            foreach (var FromPoint in Quad)
            {
                var CCSPoint = cylindricalCoordinatesSystem.GetCCSPoint(FromPoint);
                CCSPoint.z = Mathf.Max(CCSPoint.z - FarCulling, 0f);
                var OffSetToPoint = cylindricalCoordinatesSystem.GetWorldPoint(CCSPoint);

                var CCSFromPoint = cylindricalCoordinatesSystem.GetCCSPoint(FromPoint);
                CCSFromPoint.z += NierCullingOffSet;
                var OffSetFromPoint = cylindricalCoordinatesSystem.GetWorldPoint(CCSFromPoint);

                Gizmos.DrawLine(OffSetFromPoint, OffSetToPoint);
            }

            for (int Count = 0; 4 > Count; Count += 1)
            {
                (var From, var To) = GetEdge(Quad, Count);
                Gizmos.DrawLine(From, To);
            }


        }
        public static (Vector3, Vector3) GetEdge(IReadOnlyList<Vector3> Quad, int Count)
        {
            switch (Count)
            {
                default:
                case 0:
                    {
                        return (Quad[0], Quad[1]);
                    }
                case 1:
                    {
                        return (Quad[0], Quad[2]);
                    }
                case 2:
                    {
                        return (Quad[2], Quad[3]);
                    }
                case 3:
                    {
                        return (Quad[1], Quad[3]);
                    }
            }
        }

        public override void ScaleApply()
        {
            ScaleApply(new Vector3(Scale.x, Scale.y, 1), FixedAspect);
        }
    }
}
#endif
