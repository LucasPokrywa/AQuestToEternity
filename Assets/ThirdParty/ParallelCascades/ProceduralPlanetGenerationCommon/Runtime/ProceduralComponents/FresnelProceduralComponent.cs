using System;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents
{
    [Serializable]
    public class FresnelProceduralComponent : IColorMaterialComponent
    {
        private static readonly int s_fresnelColor = Shader.PropertyToID("_Fresnel_Color");
        
        [SerializeField] private float m_fresnelIntensity = 1f;
        
        public void GenerateColorForMaterial(Color color, Material material)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            
            v *= m_fresnelIntensity;
            
            Color fresnelColor = Color.HSVToRGB(h, s, v);
            material.SetColor(s_fresnelColor, fresnelColor);
        }
    }
}