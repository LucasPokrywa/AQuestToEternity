using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents
{
    [Serializable]
    public class MoonPatternFeatureComponent : IRandomizableFeature
    {
        [SerializeField] private Vector2 m_surfaceNoiseScaleRange = new Vector2(0.5f, 3f);
        
        private static readonly int s_surfaceNoiseScalePropertyId = Shader.PropertyToID("_Surface_Noise_Scale");
        
        public void Randomize(Material material)
        {
            material.SetFloat(s_surfaceNoiseScalePropertyId, Random.Range(m_surfaceNoiseScaleRange.x, m_surfaceNoiseScaleRange.y));
        }
    }
}