using System;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents
{
    [Serializable]
    public class CoronaGlowProceduralComponent : IColorComponent
    {
        [SerializeField] private StarCorona m_corona;
        public void GenerateColor(Color color)
        {
            m_corona.SetColor(color);
        }
    }
}