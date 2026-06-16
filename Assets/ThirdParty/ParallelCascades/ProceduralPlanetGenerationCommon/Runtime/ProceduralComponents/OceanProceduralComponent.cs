using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents
{
    [Serializable]
    public class OceanProceduralComponent : IColorMaterialComponent
    {
        private static readonly int s_oceanColorPropertyId = Shader.PropertyToID("_Ocean_Color");
        private static readonly int s_shallowsColorPropertyId = Shader.PropertyToID("_Shallows_Color");
        
        public void GenerateColorForMaterial(Color color, Material material)
        {
            Color oceanColor = Color.Lerp(color, Color.black,Random.Range(0.2f,0.5f));
            Color shallowsColor = color;
            
            material.SetColor(s_oceanColorPropertyId, oceanColor);
            material.SetColor(s_shallowsColorPropertyId, shallowsColor);
        }
    }
}