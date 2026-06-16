using System;
using ParallelCascades.Common.Runtime;
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
    public class StarGradientProceduralComponent : IColorMaterialComponent, IValidatableComponent
    {
        [SerializeField] private Texture2D m_colorGradientTexture;

        [BindGradientToTexture("m_colorGradientTexture", true, "m_starGradient")][SerializeField]
        private Gradient m_colorGradient = new(){colorKeys =  new GradientColorKey[2]
        {
            new() {color = Color.yellow, time = 0},
            new() {color = Color.red, time = 1}
        }};
        
        private Color m_colorQ = Color.red;
        private Color m_colorR = Color.yellow;
        
        [Tooltip("The main color will be randomly sampled from this gradient when generating the star.")]
        [SerializeField] public Gradient RandomizationColorPalette = new Gradient
        {
            colorKeys = new GradientColorKey[4]
            {
                new() {color = Color.red, time = 0},
                new() {color = Color.yellow, time = .5f},
                new() {color = Color.white, time = .75f},
                new() {color = Color.cyan, time = 1}
            }
        };
        
        private static readonly int s_colorRProperty = Shader.PropertyToID("_Color_R");
        private static readonly int s_colorQProperty = Shader.PropertyToID("_Color_Q");

        public void Validate()
        {
            // Necessary to restore the texture when exiting play mode
            if (m_colorGradientTexture)
            {
                RuntimeTextureUtilities.SetTextureFromGradient(m_colorGradient,m_colorGradientTexture);
            }
        }
        
        public Color GetMainColor() => RandomizationColorPalette.Evaluate(Random.Range(0f, 1f));
        
        public void GenerateColorForMaterial(Color color, Material material)
        {
            m_colorGradient = ColorUtilities.CreateRandomGradientFromColor(color, 3, 3, .9f, 1f);
            
            m_colorR = ColorUtilities.RandomColorSaturationFromGradient(m_colorGradient);
            
            m_colorQ = ColorUtilities.RandomColorSaturationFromGradient(m_colorGradient);
            
            if (m_colorGradientTexture != null)
            {
                RuntimeTextureUtilities.SetTextureFromGradient(m_colorGradient, m_colorGradientTexture);
            }
            
            material.SetColor(s_colorRProperty, m_colorR);
            material.SetColor(s_colorQProperty, m_colorQ);
        }
    }
}