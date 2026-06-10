using System;
using ParallelCascades.ProceduralShaders.Editor;
using ParallelCascades.ProceduralShaders.Runtime.PropertyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.ProceduralComponents
{
    /// <summary>
    /// Due to using the BindGradientToTexture attribute to bind the inspector-editable gradient to a texture in the Editor,
    /// this component needs to be named with the same name that the 'parentPropertyName' parameter in the attribute: <c>m_gradient</c>.
    /// </summary>
    [Serializable]
    public class EarthLikeLandColorsProceduralComponent : IColorComponent, IValidatableComponent
    {
        [Header("Material Properties")]
        [SerializeField] private Texture2D m_colorGradientTexture;
        
        [SerializeField] [BindGradientToTexture("m_colorGradientTexture", parentPropertyName:"m_landColor")] private Gradient m_colorGradient = new(){colorKeys =  new GradientColorKey[2]
        {
            new() {color = Color.green, time = 0},
            new() {color = Color.white, time = 1}
        }};
        
        public void GenerateColor(Color color)
        {
            // Generate land colors - not connected to main color, which would be the ocean color
            Color landColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
            m_colorGradient = GradientUtilities.RandomLandColorGradient(landColor);
            
            if (m_colorGradientTexture != null)
            {
                RuntimeTextureUtilities.SetTextureFromGradient(m_colorGradient, m_colorGradientTexture);
            }
        }

        public void Validate()
        {
            SetTextures();
        }

        private void SetTextures()
        {
            if (m_colorGradientTexture)
            {
                RuntimeTextureUtilities.SetTextureFromGradient(m_colorGradient, m_colorGradientTexture);
            }
        }
    }
}