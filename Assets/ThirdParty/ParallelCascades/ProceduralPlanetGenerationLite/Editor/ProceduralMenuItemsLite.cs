using ParallelCascades.ProceduralPlanetGenerationCommon.Editor;
using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime;
using ParallelCascades.ProceduralPlanetGenerationLite.Runtime;
using ParallelCascades.ProceduralShaders.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace ParallelCascades.ProceduralPlanetGenerationLite.Editor
{
    public static class ProceduralMenuItemsLite
    {
        private static readonly int s_colorGradient = Shader.PropertyToID("_Color_Gradient_Texture");

        [MenuItem("GameObject/Parallel Cascades/Procedural Planets Lite/Moon Lite", false, 10)]
        private static void CreateProceduralMoon(MenuCommand menuCommand)
        {
            if(!EditorAssetUtility.ChooseProjectRelativeFolderPath(out var relativeFolderPath, "Select Folder to Save Materials and Textures"))
            {
                return;
            }
            
            var go = EditorAssetUtility.CreateGameObject(menuCommand.context as GameObject, "Procedural Moon", true);
            
            SetupProceduralMoon(go, relativeFolderPath, ProceduralLiteAssetsUtility.CreateProceduralMoon(relativeFolderPath));
            
            EditorAssetUtility.SaveAssetsAndFocusFolder(relativeFolderPath);
        }
        
        private static void SetupProceduralMoon(GameObject go, string relativeFolderPath, Material material)
        {
            var meshRenderer = SetupMeshFilterRenderer(go, material, AssetGenerationUtility.GetIcoSphereMesh());
            
            Texture2D colorGradientTexture = TextureUtilities.CreateGradientTexture(relativeFolderPath, "Moon Gradient");
            material.SetTexture(s_colorGradient, colorGradientTexture);
            
            ProceduralMoonLite proceduralMoon = go.AddComponent<ProceduralMoonLite>();
            
            // This part remains even if we instantiate prefab
            SerializedObject serializedProceduralMoon = new SerializedObject(proceduralMoon);
            serializedProceduralMoon.FindProperty("m_gradient").FindPropertyRelative("m_colorGradientTexture").objectReferenceValue = colorGradientTexture;
            serializedProceduralMoon.FindProperty("m_renderer").objectReferenceValue = meshRenderer;
            serializedProceduralMoon.ApplyModifiedProperties();
            
            proceduralMoon.RandomizeEntirePlanet();
        }
        
        [MenuItem("GameObject/Parallel Cascades/Procedural Planets Lite/Earth-like Lite", false, 11)]
        private static void CreateProceduralEarthLikePlanet(MenuCommand menuCommand)
        {
            if(!EditorAssetUtility.ChooseProjectRelativeFolderPath(out var relativeFolderPath, "Select Folder to Save Materials and Textures"))
            {
                return;
            }
            
            var go = EditorAssetUtility.CreateGameObject(menuCommand.context as GameObject, "Procedural Earth-like", true);
            SetupProceduralEarth(go, relativeFolderPath);
            
            EditorAssetUtility.SaveAssetsAndFocusFolder(relativeFolderPath);
        }
        
        private static void SetupProceduralEarth(GameObject go, string relativeFolderPath)
        {
            Material material = ProceduralLiteAssetsUtility.CreateProceduralEarthMaterial(relativeFolderPath);
            var meshRenderer = SetupMeshFilterRenderer(go, material, AssetGenerationUtility.GetIcoSphereMesh());
            
            Texture2D landColorGradientTexture = TextureUtilities.CreateGradientTexture(relativeFolderPath, "Land Color Gradient");
            
            material.SetTexture(s_colorGradient, landColorGradientTexture);
            
            ProceduralEarthLikePlanetLite proceduralEarthLikePlanet = go.AddComponent<ProceduralEarthLikePlanetLite>();
            
            // This part remains even if we instantiate prefab
            SerializedObject serializedProceduralEarthLike = new SerializedObject(proceduralEarthLikePlanet);
            var landColorProperty = serializedProceduralEarthLike.FindProperty("m_landColor");
            landColorProperty.FindPropertyRelative("m_colorGradientTexture").objectReferenceValue = landColorGradientTexture;
            serializedProceduralEarthLike.FindProperty("m_renderer").objectReferenceValue = meshRenderer;
            
            PlanetGlow planetGlow = go.AddComponent<PlanetGlow>();
            SerializedObject serializedPlanetGlow = new SerializedObject(planetGlow);
            Material planetGlowMaterial = ProceduralLiteAssetsUtility.CreatePlanetGlowMaterial(relativeFolderPath);
            serializedPlanetGlow.FindProperty("m_material").objectReferenceValue = planetGlowMaterial;
            serializedPlanetGlow.ApplyModifiedProperties();
            planetGlow.TryInitializePostFXGlow();
            serializedProceduralEarthLike.FindProperty("m_glow").FindPropertyRelative("m_glow").objectReferenceValue = planetGlow;

            serializedProceduralEarthLike.ApplyModifiedProperties();
            
            proceduralEarthLikePlanet.RandomizeEntirePlanet();
        }
        
        [MenuItem("GameObject/Parallel Cascades/Procedural Planets Lite/Gas Giant with Storms", false, 12)]
        private static void CreateProceduralGasGiantWithStorms(MenuCommand menuCommand)
        {
            if(!EditorAssetUtility.ChooseProjectRelativeFolderPath(out var relativeFolderPath, "Select Folder to Save Materials and Textures"))
            {
                return;
            }
            
            var go = EditorAssetUtility.CreateGameObject(menuCommand.context as GameObject, "Procedural Gas Giant", true);
            SetupProceduralGasGiant<ProceduralGasGiantWithStorms>(go, relativeFolderPath, ProceduralLiteAssetsUtility.CreateProceduralGasGiantWithStormsMaterial(relativeFolderPath));
            
            EditorAssetUtility.SaveAssetsAndFocusFolder(relativeFolderPath);
        }
        
        [MenuItem("GameObject/Parallel Cascades/Procedural Planets Lite/Gas Giant Lite", false, 12)]
        private static void CreateProceduralGasGiant(MenuCommand menuCommand)
        {
            if(!EditorAssetUtility.ChooseProjectRelativeFolderPath(out var relativeFolderPath, "Select Folder to Save Materials and Textures"))
            {
                return;
            }
            
            var go = EditorAssetUtility.CreateGameObject(menuCommand.context as GameObject, "Procedural Gas Giant", true);
            SetupProceduralGasGiant<ProceduralGasGiantLite>(go, relativeFolderPath, ProceduralLiteAssetsUtility.CreateProceduralGasGiantMaterial(relativeFolderPath));
            
            EditorAssetUtility.SaveAssetsAndFocusFolder(relativeFolderPath);
        }
        
        [MenuItem("GameObject/Parallel Cascades/Procedural Planets Lite/Gas Giant Lite (Simplified)", false, 12)]
        private static void CreateProceduralGasGiantSimplified(MenuCommand menuCommand)
        {
            if(!EditorAssetUtility.ChooseProjectRelativeFolderPath(out var relativeFolderPath, "Select Folder to Save Materials and Textures"))
            {
                return;
            }
            
            var go = EditorAssetUtility.CreateGameObject(menuCommand.context as GameObject, "Procedural Gas Giant", true);
            SetupProceduralGasGiant<ProceduralGasGiantSimplifiedLite>(go, relativeFolderPath, ProceduralLiteAssetsUtility.CreateProceduralGasGiantMaterialSimplified(relativeFolderPath));
            
            EditorAssetUtility.SaveAssetsAndFocusFolder(relativeFolderPath);
        }
        
        private static void SetupProceduralGasGiant<T>(GameObject go, string relativeFolderPath, Material material) where T: ProceduralBodyBase
        {
            var meshRenderer = SetupMeshFilterRenderer(go, material, AssetGenerationUtility.GetIcoSphereMesh());

            Texture2D colorGradientTexture = TextureUtilities.CreateGradientTexture(relativeFolderPath, "Gas Giant Gradient");
            material.SetTexture(s_colorGradient, colorGradientTexture);
            
            PlanetGlow planetGlow = go.AddComponent<PlanetGlow>();
            SerializedObject serializedPlanetGlow = new SerializedObject(planetGlow);
            Material planetGlowMaterial = ProceduralLiteAssetsUtility.CreatePlanetGlowMaterial(relativeFolderPath);
            serializedPlanetGlow.FindProperty("m_material").objectReferenceValue = planetGlowMaterial;
            serializedPlanetGlow.ApplyModifiedProperties();
            planetGlow.TryInitializePostFXGlow();
            
            T proceduralGasGiant = go.AddComponent<T>();
            SerializedObject serializedGasGiant = new SerializedObject(proceduralGasGiant);
            serializedGasGiant.FindProperty("m_gradient").FindPropertyRelative("m_colorGradientTexture").objectReferenceValue = colorGradientTexture;
            serializedGasGiant.FindProperty("m_glow").FindPropertyRelative("m_glow").objectReferenceValue = planetGlow;
            serializedGasGiant.FindProperty("m_renderer").objectReferenceValue = meshRenderer;
            serializedGasGiant.ApplyModifiedProperties();
            
            proceduralGasGiant.RandomizeEntirePlanet();
        }
        

        
        [MenuItem("GameObject/Parallel Cascades/Procedural Planets Lite/Star Lite", false, 13)]
        private static void CreateProceduralStar(MenuCommand menuCommand)
        {
            if(!EditorAssetUtility.ChooseProjectRelativeFolderPath(out var relativeFolderPath, "Select Folder to Save Materials and Textures"))
            {
                return;
            }
            
            var go = EditorAssetUtility.CreateGameObject(menuCommand.context as GameObject, "Procedural Star", true);
            SetupProceduralStar(go, relativeFolderPath);
            
            EditorAssetUtility.SaveAssetsAndFocusFolder(relativeFolderPath);
        }
        
        private static void SetupProceduralStar(GameObject go, string relativeFolderPath)
        {
            ProceduralStarLite proceduralStar = go.AddComponent<ProceduralStarLite>();
            
            StarCorona starCorona = go.AddComponent<StarCorona>();
            Material starCoronaMaterial = ProceduralLiteAssetsUtility.CreateStarCoronaMaterial(relativeFolderPath);
            
            SerializedObject serializedStarCorona = new SerializedObject(starCorona);
            serializedStarCorona.FindProperty("m_material").objectReferenceValue = starCoronaMaterial;
            serializedStarCorona.ApplyModifiedProperties();
            starCorona.TryInitializePostFXGlow();

            Texture2D colorGradientTexture = TextureUtilities.CreateGradientTexture(relativeFolderPath, "Procedural Star Gradient");
            Material starMaterial =
                ProceduralLiteAssetsUtility.CreateProceduralStarMaterial(relativeFolderPath);

            starMaterial.SetTexture(s_colorGradient, colorGradientTexture);
            SerializedObject serializedStar = new SerializedObject(proceduralStar);
            serializedStar.FindProperty("m_starGradient").FindPropertyRelative("m_colorGradientTexture").objectReferenceValue = colorGradientTexture;
            serializedStar.FindProperty("m_coronaGlow").FindPropertyRelative("m_corona").objectReferenceValue = starCorona;
            
            var meshRenderer = SetupMeshFilterRenderer(go, starMaterial, AssetGenerationUtility.GetIcoSphereMesh(), ShadowCastingMode.Off);
            
            serializedStar.FindProperty("m_renderer").objectReferenceValue = meshRenderer;
            
            serializedStar.ApplyModifiedProperties();
            
            proceduralStar.RandomizeEntirePlanet();
        } 
        
        [MenuItem("GameObject/Parallel Cascades/Procedural Planets Lite/Asteroid Ring Lite", false, 12)]
        private static void CreateProceduralАsteroidRing(MenuCommand menuCommand)
        {
            if(!EditorAssetUtility.ChooseProjectRelativeFolderPath(out var relativeFolderPath, "Select Folder to Save Materials and Textures"))
            {
                return;
            }
            
            var go = EditorAssetUtility.CreateGameObject(menuCommand.context as GameObject, "Procedural Asteroid Ring");
            SetupProceduralAsteroidRing(go, relativeFolderPath, ProceduralLiteAssetsUtility.CreateAsteroidRingMaterial(relativeFolderPath));
            
            
            EditorAssetUtility.SaveAssetsAndFocusFolder(relativeFolderPath);
        }
        
        [MenuItem("GameObject/Parallel Cascades/Procedural Planets Lite/Asteroid Ring Lite Improved", false, 12)]
        private static void CreateProceduralАsteroidRingImproved(MenuCommand menuCommand)
        {
            if(!EditorAssetUtility.ChooseProjectRelativeFolderPath(out var relativeFolderPath, "Select Folder to Save Materials and Textures"))
            {
                return;
            }
            
            var go = EditorAssetUtility.CreateGameObject(menuCommand.context as GameObject, "Procedural Asteroid Ring");
            SetupProceduralAsteroidRing(go, relativeFolderPath, ProceduralLiteAssetsUtility.CreateAsteroidRingLiteImprovedMaterial(relativeFolderPath));
            
            
            EditorAssetUtility.SaveAssetsAndFocusFolder(relativeFolderPath);
        }
        
        private static void SetupProceduralAsteroidRing(GameObject go, string relativeFolderPath, Material material)
        {
            ProceduralAsteroidRingLite proceduralAsteroidRing = go.AddComponent<ProceduralAsteroidRingLite>();
            
            var meshRenderer = SetupMeshFilterRenderer(go, material, AssetGenerationUtility.GetDoubleSidedPlaneMesh(), ShadowCastingMode.Off);
            
            Texture2D colorGradientTexture = TextureUtilities.CreateGradientTexture(relativeFolderPath, "Asteroid Ring Gradient");
            material.SetTexture(s_colorGradient, colorGradientTexture);
            SerializedObject serializedAsteroidRing = new SerializedObject(proceduralAsteroidRing);
            serializedAsteroidRing.FindProperty("m_gradient").FindPropertyRelative("m_colorGradientTexture").objectReferenceValue = colorGradientTexture;
            serializedAsteroidRing.FindProperty("m_renderer").objectReferenceValue = meshRenderer;
            serializedAsteroidRing.ApplyModifiedProperties();
            
            proceduralAsteroidRing.RandomizeEntirePlanet();
        }
        
        private static MeshRenderer SetupMeshFilterRenderer(GameObject go, Material material, Mesh mesh, ShadowCastingMode shadowCastingMode = ShadowCastingMode.On)
        {
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
            
            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = material;
            meshRenderer.shadowCastingMode = shadowCastingMode;
            
            return meshRenderer;
        }
    }
}