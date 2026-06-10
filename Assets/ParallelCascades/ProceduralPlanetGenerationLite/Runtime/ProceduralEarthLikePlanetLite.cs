using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime;
using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationLite.Runtime
{
    public class ProceduralEarthLikePlanetLite : ProceduralBodyBase
    {
        [Header("Material Randomization Properties")]
        [SerializeField] private EarthLikeLandColorsProceduralComponent m_landColor;
        [SerializeField] private EarthSurfaceProceduralFeatureComponent m_surface;
        [SerializeField] private Gradient m_oceanColorsPalette = new  Gradient(){
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(new Color(0f, 0.1f, 0.4f), 0f),
                new GradientColorKey(new Color(0f, 0.6f, 0.5f), 0.5f),
                new GradientColorKey(new Color(0.2f, 0.15f, 0.15f), 1f)
            }
        };
        [SerializeField] private OceanProceduralComponent m_ocean;
        [SerializeField] private FresnelProceduralComponent m_fresnel;
        [SerializeField] private PlanetGlowProceduralComponent m_glow;
        

        protected override IProceduralComponent[] ProceduralComponents =>
            new IProceduralComponent[]
            {
                m_surface,
                m_landColor,
                m_ocean,
                m_fresnel,
                m_glow
            };
        
        protected override Color GetMainColor() => m_oceanColorsPalette.Evaluate(Random.Range(0f, 1f));

    }
}