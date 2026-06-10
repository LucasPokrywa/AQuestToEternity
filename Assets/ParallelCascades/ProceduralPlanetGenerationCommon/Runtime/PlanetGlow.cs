using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime
{
    [ExecuteInEditMode]
    public class PlanetGlow : PostFXGlowEffect
    {        
        protected static readonly int GlowColor = Shader.PropertyToID("_Glow_Color");

        [SerializeField][ColorUsage(false,true)]
        protected Color m_color = Color.white;

        // Overwrites base class default value
        private void Reset()
        {
            m_effectScale = 1.5f;
            m_densityFalloff = 12.5f;
        }

        protected override void UpdateMaterial()
        {
            if (m_material == null)
            {
                return;
            }
            
            var meshAdjustedScale = ObjectScale * m_meshRadius;
            var meshAdjustedEffectScale = m_effectScale * m_meshRadius;
            
            m_material.SetFloat(EffectRadius, meshAdjustedScale * (1+meshAdjustedEffectScale));
            m_material.SetFloat(ObjectRadius, meshAdjustedScale);            
            m_material.SetFloat(DensityFalloff, m_densityFalloff);
            m_material.SetColor(GlowColor, m_color);
        }

        public void SetColor(Color glowColor)
        {
#if UNITY_EDITOR
            // This allows us to record the change in the editor, when using Editor UI
            if (!Application.isPlaying)
            {
                UnityEditor.Undo.RecordObject(this, "Set Glow Color");
            }
#endif
    
            m_color = glowColor;
    
            if (m_material != null)
            {
                m_material.SetColor(GlowColor, m_color);
            }
    
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }
    }
}