using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents
{
    [Serializable]
    public class StarPatternProceduralFeatureComponent : IRandomizableFeature
    {
        [SerializeField] private Vector2 m_patternScaleRange = new(1f, 5f);
        [SerializeField] private Vector2 m_warpAmountRange = new(0.1f, 3f);
        [SerializeField] private Vector2 m_flowSpeedRange = new(0.025f, 0.2f);
        
        private static readonly int s_scale = Shader.PropertyToID("_Scale");
        private static readonly int s_warpAmount = Shader.PropertyToID("_Warp_Amount");
        private static readonly int s_flowSpeed = Shader.PropertyToID("_Flow_Speed");
        
        public void Randomize(Material material)
        { 
            material.SetFloat(s_scale, Random.Range(m_patternScaleRange.x, m_patternScaleRange.y));
            material.SetFloat(s_warpAmount, Random.Range(m_warpAmountRange.x, m_warpAmountRange.y));
            material.SetFloat(s_flowSpeed, Random.Range(m_flowSpeedRange.x, m_flowSpeedRange.y));
        }
    }
}