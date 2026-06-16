using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime;
using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationLite.Runtime
{
    public class ProceduralStarLite : ProceduralBodyBase
    {
        [Header("Material Randomization Properties")]
        [SerializeField] private StarGradientProceduralComponent m_starGradient;
        [SerializeField] private StarPatternProceduralFeatureComponent m_starPattern;
        [SerializeField] private FresnelProceduralComponent m_fresnel;
        [SerializeField] private CoronaGlowProceduralComponent m_coronaGlow;
        
        protected override IProceduralComponent[] ProceduralComponents => new IProceduralComponent[]
        {
            m_fresnel,
            m_starPattern,
            m_starGradient,
            m_coronaGlow
        };
        
        protected override Color GetMainColor() => m_starGradient.GetMainColor();
    }
}