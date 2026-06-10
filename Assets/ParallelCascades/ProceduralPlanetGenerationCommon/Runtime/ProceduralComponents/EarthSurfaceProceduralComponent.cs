using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents
{
    [Serializable]
    public class EarthSurfaceProceduralFeatureComponent : IRandomizableFeature
    {
        [SerializeField] private Vector2 m_continentScaleRange = new Vector2(0.5f, 2f);
        [SerializeField] private Vector2 m_elevationScaleRange = new Vector2(0.6f, 1.5f);
        [SerializeField] private Vector2 m_oceanLandThresholdRange = new Vector2(0.0f,0.4f);
        [SerializeField] private Vector2 m_iceCapsEdgeThresholdRange = new Vector2(0.1f, 0.45f);
        
        private static readonly int s_continentScalePropertyId = Shader.PropertyToID("_Continent_Scale");
        private static readonly int s_elevationScalePropertyId = Shader.PropertyToID("_Elevation_Scale");
        private static readonly int s_oceanLandThresholdPropertyId = Shader.PropertyToID("_Ocean_Land_Threshold");
        private static readonly int s_randomizationOffsetPropertyId = Shader.PropertyToID("_Randomization_Offset");
        private static readonly int s_iceCapsEdgeThresholdPropertyId = Shader.PropertyToID("_Ice_Caps_Edge_Threshold");
        
        
        public void Randomize(Material material)
        {
            material.SetFloat(s_continentScalePropertyId, Random.Range(m_continentScaleRange.x, m_continentScaleRange.y));
            material.SetFloat(s_elevationScalePropertyId, Random.Range(m_elevationScaleRange.x, m_elevationScaleRange.y));
            material.SetFloat(s_iceCapsEdgeThresholdPropertyId, Random.Range(m_iceCapsEdgeThresholdRange.x, m_iceCapsEdgeThresholdRange.y));
            material.SetFloat(s_oceanLandThresholdPropertyId, Random.Range(m_oceanLandThresholdRange.x, m_oceanLandThresholdRange.y));
            material.SetVector(s_randomizationOffsetPropertyId, Random.insideUnitSphere * Random.Range(0, 1000f));
        }
    }
}