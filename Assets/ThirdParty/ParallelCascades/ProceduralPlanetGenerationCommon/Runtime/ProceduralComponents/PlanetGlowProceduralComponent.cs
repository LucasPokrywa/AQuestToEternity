using System;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents
{
    [Serializable]
    public class PlanetGlowProceduralComponent : IColorComponent
    {
        [SerializeField] private PlanetGlow m_glow;
        
        public void GenerateColor(Color color)
        {
            if (m_glow)
            {
                m_glow.SetColor(color);
            }
        }
    }
}