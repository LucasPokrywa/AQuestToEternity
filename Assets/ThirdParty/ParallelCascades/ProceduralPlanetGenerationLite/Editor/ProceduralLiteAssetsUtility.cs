using ParallelCascades.ProceduralPlanetGenerationCommon.Editor;
using UnityEngine;
using UnityEngine.VFX;

namespace ParallelCascades.ProceduralPlanetGenerationLite.Editor
{
    public static class ProceduralLiteAssetsUtility
    {
        public static Material CreateProceduralMoon(string projectRelativeFolderPath)
        {
            return EditorAssetUtility.CreateMaterial(projectRelativeFolderPath, "Parallel Cascades/Procedural Planet Generation Lite/Moon Lite", "Procedural Moon Lite");
        }

        public static Material CreateProceduralEarthMaterial(string projectRelativeFolderPath)
        {
            return EditorAssetUtility.CreateMaterial(projectRelativeFolderPath, "Parallel Cascades/Procedural Planet Generation Lite/Earth-like Lite", "Procedural Earth Planet Lite");
        }

        public static Material CreateProceduralStarMaterial(string projectRelativeFolderPath)
        {
            return EditorAssetUtility.CreateMaterial(projectRelativeFolderPath, "Parallel Cascades/Procedural Planet Generation Lite/Star Lite", "Star Lite");
        }
        
        public static Material CreateProceduralGasGiantWithStormsMaterial(string projectRelativeFolderPath)
        {
            return EditorAssetUtility.CreateMaterial(projectRelativeFolderPath, "Parallel Cascades/Procedural Planet Generation Lite/Gas Giant with Storms", "Gas Giant with Storms");
        }

        public static Material CreateProceduralGasGiantMaterial(string projectRelativeFolderPath)
        {
            return EditorAssetUtility.CreateMaterial(projectRelativeFolderPath, "Parallel Cascades/Procedural Planet Generation Lite/Gas Giant Lite", "Gas Giant Lite");
        }
        
        public static Material CreateProceduralGasGiantMaterialSimplified(string projectRelativeFolderPath)
        {
            return EditorAssetUtility.CreateMaterial(projectRelativeFolderPath, "Parallel Cascades/Procedural Planet Generation Lite/Gas Giant Lite (Simplified)", "Gas Giant Simplified");
        }

        public static Material CreatePlanetGlowMaterial(string projectRelativeFolderPath)
        {
            return EditorAssetUtility.CreateMaterial(projectRelativeFolderPath, "Parallel Cascades/Procedural Planet Generation/Planet Glow", "Planet Glow");
        }

        public static Material CreateStarCoronaMaterial(string projectRelativeFolderPath)
        {
            return EditorAssetUtility.CreateMaterial(projectRelativeFolderPath, "Parallel Cascades/Procedural Planet Generation/Star Corona", "Star Corona");
        }
        
        public static Material CreateAsteroidRingLiteImprovedMaterial(string projectRelativeFolderPath)
        {
            return EditorAssetUtility.CreateMaterial(projectRelativeFolderPath, "Parallel Cascades/Procedural Planet Generation Lite/Asteroid Ring Lite Improved", "Asteroid Ring Lite Improved");
        }

        public static Material CreateAsteroidRingMaterial(string projectRelativeFolderPath)
        {
            return EditorAssetUtility.CreateMaterial(projectRelativeFolderPath, "Parallel Cascades/Procedural Planet Generation Lite/Asteroid Ring Lite", "Asteroid Ring Lite");
        }

        public static VisualEffectAsset GetStarCoronaVFXAsset()
        {
            return Resources.Load<VisualEffectAsset>("VFX/Star Corona VFX");
        }
    }
}