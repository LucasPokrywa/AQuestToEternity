using System;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime
{
    [ExecuteInEditMode]
    public class StarCorona : PostFXGlowEffect
    {        
        protected static readonly int CoronaColor = Shader.PropertyToID("_Corona_Color");
        
        [SerializeField][ColorUsage(false,true)]
        protected Color _color = Color.yellow;

        private void Reset()
        {
            m_densityFalloff = 20f;
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
            m_material.SetColor(CoronaColor, _color);
        }

        public void SetColor(Color glowColor)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.Undo.RecordObject(this, "Set Glow Color");
            }
#endif
            _color = glowColor;
            
            m_material.SetColor(CoronaColor, _color);
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }
        
    }
}