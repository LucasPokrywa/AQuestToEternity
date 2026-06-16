using UnityEngine;
using UnityEngine.VFX;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Editor
{
    public static class AssetGenerationUtility
    {
        public static Mesh GetIcoSphereMesh()
        {
            return Resources.Load<Mesh>("Meshes/icosphere_high");
        }

        public static Mesh GetDoubleSidedPlaneMesh()
        {
            return Resources.Load<Mesh>("Meshes/double_sided_plane");
        }
    }
}