using System;
using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParallelCascades.ProceduralPlanetGenerationLite.Runtime.ProceduralComponents
{
    [Serializable]
    public class WhorlsProceduralFeatureComponent : IRandomizableFeature
    {
        [Header("Randomization Properties")]
        [SerializeField] private Vector2 m_whorlDensityRange = new Vector2(2f, 10f);
        [SerializeField] private Vector2 m_whorlSquishRange = new Vector2(1f, 2f);
        [SerializeField] private Vector2 m_whorlTwistinessRange = new Vector2(.5f, 1f);
        [SerializeField] private Vector2 m_whorlSizeToDensityRatioRange = new Vector2(0.04f, 0.09f);
        
        private static readonly int s_whorlDensity = Shader.PropertyToID("_Whorl_Density");
        private static readonly int s_whorlSquish = Shader.PropertyToID("_Whorl_Squish");
        private static readonly int s_whorlTwistiness = Shader.PropertyToID("_Whorl_Twistiness");
        private static readonly int s_whorlSizeModifier = Shader.PropertyToID("_Whorl_Size_Modifier");
        private static readonly int s_whorlOffset = Shader.PropertyToID("_Whorl_Offset");
        
        public void Randomize(Material material)
        {
            int whorlDensity = (int)Random.Range(m_whorlDensityRange.x, m_whorlDensityRange.y);
            
            material.SetFloat(s_whorlDensity, whorlDensity);
            material.SetFloat(s_whorlSquish, Random.Range(m_whorlSquishRange.x, m_whorlSquishRange.y));
            material.SetFloat(s_whorlTwistiness, Random.Range(m_whorlTwistinessRange.x, m_whorlTwistinessRange.y));
            var density = Random.Range(m_whorlSizeToDensityRatioRange.x, m_whorlSizeToDensityRatioRange.y) * whorlDensity;
            material.SetFloat(s_whorlSizeModifier, density);
            material.SetVector(s_whorlOffset, new Vector2(Random.value*4f, Random.value*4f));
        }
    }
}