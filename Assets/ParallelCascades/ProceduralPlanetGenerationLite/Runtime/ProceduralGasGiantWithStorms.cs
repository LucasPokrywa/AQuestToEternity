using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime;
using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents;
using ParallelCascades.ProceduralPlanetGenerationLite.Runtime.ProceduralComponents;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationLite.Runtime
{
    public class ProceduralGasGiantWithStorms : ProceduralBodyBase
    {
        [Header("Material Randomization Properties")]
        [SerializeField] private GradientProceduralComponent m_gradient;
        [SerializeField] private GasGiantPatternProceduralFeatureComponent m_gasGiantPattern;
        [SerializeField] private WhorlsProceduralFeatureComponent m_whorls;
        [SerializeField] private FresnelProceduralComponent m_fresnel;
        [SerializeField] private PlanetGlowProceduralComponent m_glow;
        
        protected override IProceduralComponent[] ProceduralComponents => new IProceduralComponent[]
        {
            m_fresnel,
            m_gradient,
            m_whorls,
            m_gasGiantPattern,
            m_glow
        };
        
        protected override Color GetMainColor() => Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);

    }
}