using System;
using ParallelCascades.Common.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime
{
    /// <summary>
    /// Empty marker interface for procedural components. It does not do anything on its own, but allows us
    /// to declare a list of these components in any ProceduralBodyBase implementation. We then iterate through the list
    /// and check for specific child interfaces like IColorComponent, IRandomizableFeaturesComponent etc. to call the relevant methods.
    /// </summary>
    public interface IProceduralComponent { }
    
    public interface IColorComponent : IProceduralComponent
    {
        void GenerateColor(Color color);
    }
    
    public interface IColorMaterialComponent : IProceduralComponent
    {
        void GenerateColorForMaterial(Color color, Material material);
    }
    
    public interface IRandomizableFeature : IProceduralComponent
    {
        void Randomize(Material material);
    }

    /// <summary>
    /// Called when OnValidate is called on the ProceduralBodyBase
    /// </summary>
    public interface IValidatableComponent : IProceduralComponent
    {
        void Validate();
    }
    
    /// <summary>
    /// This is an implementation of the Component Pattern - this is the base container class for procedural celestial bodies.
    /// It describes the common functionality that all bodies should have, and children should only specify
    /// the list of modules they use. Each component handles a different domain - color, noise, glow, fresnel effect generation etc.
    /// Components should be small and independent modules that can be reused in different combinations to create different types of bodies.
    /// Components should not depend on or know of each other, they only get passed data from the base class.
    /// </summary>
    public abstract class ProceduralBodyBase : MonoBehaviour
    {
        [InspectorButton("RandomizeOffset", "Reroll Appearance")]
        [InspectorButton("RandomizeColor", "Randomize Color")]
        [InspectorButton("RandomizeFeatures", "Randomize Features")]
        [InspectorButton("RandomizeEntirePlanet", "Randomize Entire Planet")]
        [SerializeField] private MeshRenderer m_renderer;
        
        [InspectorButton("SetColorFromSeed", "Set Color From Seed")]
        [SerializeField] protected int m_colorSeed;
        [InspectorButton("SetFeaturesFromSeed", "Set Features From Seed")]
        [SerializeField] protected int m_featuresSeed;
        
        protected abstract IProceduralComponent[] ProceduralComponents { get; }

        private Material Material
        {
            get
            {
                if (Application.isPlaying)
                {
                    return m_renderer.material;
                }
                else
                {
                    return m_renderer.sharedMaterial;
                }
            }
        }

        private void RandomizeColorSeed() => m_colorSeed = DateTime.Now.Millisecond;
        private void RandomizeFeaturesSeed() => m_featuresSeed = DateTime.Now.Millisecond;

        private void OnValidate()
        {
            foreach (var component in ProceduralComponents)
            {
                if (component is IValidatableComponent v)
                {
                    v.Validate();
                }
            }
        }
        
        private static readonly int s_offsetPropertyId = Shader.PropertyToID("_Randomization_Offset");
        
        /// <summary>
        /// Editor button is named "Reroll Appearance"
        /// </summary>
        public void RandomizeOffset()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(Material, "Randomize Offset");
#endif
            Material.SetVector(s_offsetPropertyId, Random.insideUnitSphere * 1000f);
        }
        
        public void SetColorFromSeed()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(Material, "Set Color From Seed");
#endif
            GenerateColor();
        }
        
        public void SetFeaturesFromSeed()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(Material, "Set Features From Seed");
#endif
            GenerateFeatures();
        }

        public void SetSeeds(int seed)
        {
            m_colorSeed = seed;
            m_featuresSeed = seed;
        }

        public void RandomizeColor()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(Material, "Randomize Color");
#endif
            RandomizeColorSeed();
            GenerateColor();
        }
        
        public void RandomizeFeatures()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(Material, "Randomize Features");
#endif
            RandomizeFeaturesSeed();
            GenerateFeatures();
        }
        
        public void RandomizeEntirePlanet()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(Material, "Randomize Entire Planet");
#endif
            RandomizeColorSeed();
            RandomizeFeaturesSeed();
            GenerateColor();
            GenerateFeatures();
        }
        
        private void GenerateColor()
        {
            Random.InitState(m_colorSeed);
            Color mainColor = GetMainColor();

            foreach (var component in ProceduralComponents)
            {
                if (component is IColorComponent colorComponent)
                {
                    colorComponent.GenerateColor(mainColor);
                }

                if (component is IColorMaterialComponent colorMaterialComponent)
                {
                    colorMaterialComponent.GenerateColorForMaterial(mainColor, Material);
                }
            }
        }

        public void GenerateFeatures()
        {
            Random.InitState(m_featuresSeed);

            foreach (var component in ProceduralComponents)
            {
                if (component is IRandomizableFeature noiseComponent)
                {
                    noiseComponent.Randomize(Material);
                }
            }
        }

        protected abstract Color GetMainColor();
    }
}