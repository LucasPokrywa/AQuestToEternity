using UnityEngine;

namespace ParallelCascades.ProceduralShaders.Runtime.PropertyAttributes
{
    /// <summary>
    /// If this attribute is applied to a field in a serializable class that is itself a field in a MonoBehaviour or ScriptableObject,
    /// make sure the name of the class field matches the 'parentPropertyName' parameter in the attribute.
    /// </summary>
    public class BindGradientToTextureAttribute : PropertyAttribute
    {
        public string TexturePropertyName { get; }
        
        public string ParentPropertyName { get; }
        
        public float SaveDelay { get; }
        
        public bool HDR { get; }
        
        public ColorSpace ColorSpace { get; }

        public BindGradientToTextureAttribute(string texturePropertyName, bool hdr = false, string parentPropertyName = null, ColorSpace colorSpace = ColorSpace.Gamma, float saveDelay = 0.5f)
        {
            TexturePropertyName = texturePropertyName;
            HDR = hdr;
            ParentPropertyName = parentPropertyName;
            ColorSpace = colorSpace;
            SaveDelay = saveDelay;
        }

    }
}