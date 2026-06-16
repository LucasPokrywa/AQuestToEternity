using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents
{
    [Serializable]
    public class AsteroidRingShapeProceduralFeatureComponent : IRandomizableFeature
    {
        [Tooltip("X - Inner Radius Min, Y - Inner Max/Outer Min, Z - Outer Max")]
        [SerializeField] private Vector3 m_radiusRange = new Vector3(0.125f, 0.3f, 0.5f);
        [SerializeField] private Vector2 m_fadeStrengthRange = new Vector2(1f, 3f);
        [SerializeField] private Vector2 m_flowSpeedRange = new Vector2(0.005f, 0.02f);
        
        private static readonly int s_innerRadius = Shader.PropertyToID("_Inner_Radius");
        private static readonly int s_outerRadius = Shader.PropertyToID("_Outer_Radius");
        private static readonly int s_fadeStrength = Shader.PropertyToID("_Fade_Strength");
        private static readonly int s_flowSpeed = Shader.PropertyToID("_Flow_Speed");
        
        public void Randomize(Material material)
        {
            material.SetFloat(s_innerRadius, Random.Range(m_radiusRange.x, m_radiusRange.y));
            material.SetFloat(s_outerRadius, Random.Range(m_radiusRange.y, m_radiusRange.z));
            material.SetFloat(s_fadeStrength, Random.Range(m_fadeStrengthRange.x, m_fadeStrengthRange.y));
            material.SetFloat(s_flowSpeed, Random.Range(m_flowSpeedRange.x, m_flowSpeedRange.y));
        }
    }
}