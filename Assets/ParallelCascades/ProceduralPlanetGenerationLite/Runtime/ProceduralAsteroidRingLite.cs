using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime;
using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationLite.Runtime
{
    public class ProceduralAsteroidRingLite : ProceduralBodyBase
    {
        [SerializeField] private GradientProceduralComponent m_gradient;
        [SerializeField] private AsteroidRingShapeProceduralFeatureComponent m_asteroidRingShape;
        
        protected override IProceduralComponent[] ProceduralComponents => new IProceduralComponent[]
        {
            m_gradient,
            m_asteroidRingShape
        };
        
        protected override Color GetMainColor() => Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);

    }
}