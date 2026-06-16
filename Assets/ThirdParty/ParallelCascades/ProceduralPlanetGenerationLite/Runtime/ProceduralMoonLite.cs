using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime;
using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationLite.Runtime
{
    public class ProceduralMoonLite : ProceduralBodyBase
    {
        [SerializeField] private MoonPatternFeatureComponent m_moonPatternFeatureComponent;
        [SerializeField] private GradientProceduralComponent m_gradient;

        protected override IProceduralComponent[] ProceduralComponents => new IProceduralComponent[]
        {
            m_moonPatternFeatureComponent,
            m_gradient
        };
        
        protected override Color GetMainColor() => Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);

    }
}