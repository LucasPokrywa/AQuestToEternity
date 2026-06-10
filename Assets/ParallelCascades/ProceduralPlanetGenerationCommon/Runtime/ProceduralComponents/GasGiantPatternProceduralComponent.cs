using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents
{
    [Serializable]
    public class GasGiantPatternProceduralFeatureComponent : IRandomizableFeature
    {
        [SerializeField] private Vector2 m_patternScaleRange = new Vector2(0.05f, 1f);
        [SerializeField] private Vector2 m_planetFlowSpeedRange = new Vector2(0.001f, 0.005f);
        [SerializeField] private Vector2 m_zScalingRange = new Vector2(0.005f, 0.15f);
        [SerializeField] private Vector2 m_xScalingRange = new Vector2(0.005f, 0.15f);
        [SerializeField] private Vector2 m_yGradientScaleRange = new Vector2(0f, 1f);
        [SerializeField] private Vector2 m_yGradientStrengthRange = new Vector2(0.01f, 0.1f);
        
        private static readonly int s_scalePropertyId = Shader.PropertyToID("_Scale");
        private static readonly int s_flowSpeedPropertyId = Shader.PropertyToID("_Flow_Speed");
        private static readonly int s_zScalingPropertyId = Shader.PropertyToID("_Z_Scaling");
        private static readonly int s_xScalingPropertyId = Shader.PropertyToID("_X_Scaling");
        private static readonly int s_yGradientScalePropertyId = Shader.PropertyToID("_Y_Gradient_Scale");
        private static readonly int s_yGradientStrengthPropertyId = Shader.PropertyToID("_Y_Gradient_Strength");
        private static readonly int s_offsetPropertyId = Shader.PropertyToID("_Offset");
        
        public void Randomize(Material material)
        {
            material.SetFloat(s_scalePropertyId, Random.Range(m_patternScaleRange.x, m_patternScaleRange.y));
            material.SetFloat(s_flowSpeedPropertyId, Random.Range(m_planetFlowSpeedRange.x, m_planetFlowSpeedRange.y));
            material.SetFloat(s_zScalingPropertyId, Random.Range(m_zScalingRange.x, m_zScalingRange.y));
            material.SetFloat(s_xScalingPropertyId, Random.Range(m_xScalingRange.x, m_xScalingRange.y));
            material.SetFloat(s_yGradientScalePropertyId, Random.Range(m_yGradientScaleRange.x, m_yGradientScaleRange.y));
            material.SetFloat(s_yGradientStrengthPropertyId, Random.Range(m_yGradientStrengthRange.x, m_yGradientStrengthRange.y));
            material.SetVector(s_offsetPropertyId, new Vector3(Random.value, Random.value, Random.value));
        }

    }
}