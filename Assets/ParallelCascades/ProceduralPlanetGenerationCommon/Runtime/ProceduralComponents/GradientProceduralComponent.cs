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
    public class GradientProceduralComponent : IColorComponent, IValidatableComponent
    {
        [SerializeField] private Texture2D m_colorGradientTexture;

        [SerializeField][BindGradientToTexture("m_colorGradientTexture", parentPropertyName: "m_gradient")] 
        private Gradient m_colorGradient = new(){colorKeys =  new GradientColorKey[2]
        {
            new() {color = Color.white, time = 0},
            new() {color = Color.black, time = 1}
        }};
        
        [SerializeField] private AnimationCurve m_twoColorGradientChanceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        public void Validate()
        {
            // Necessary to restore the texture when exiting play mode
            if (m_colorGradientTexture)
            {
                RuntimeTextureUtilities.SetTextureFromGradient(m_colorGradient,m_colorGradientTexture);
            }
        }
        
        public void GenerateColor(Color color)
        {
            float twoColorGradientChance = m_twoColorGradientChanceCurve.Evaluate(Random.value);
            float randomValue = Random.value;
            if (randomValue < twoColorGradientChance)
            {
                Color colorB = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
                m_colorGradient = ColorUtilities.CreateRandomGradientFromTwoColors(color, colorB, 8, 8, .5f, 2f);
            }
            else
            {
                m_colorGradient = ColorUtilities.CreateRandomGradientFromColor(color, 2, 8, .5f, 2f); 
            }
            
            if (m_colorGradientTexture != null)
            {
                RuntimeTextureUtilities.SetTextureFromGradient(m_colorGradient, m_colorGradientTexture);
            }
        }
    }
}