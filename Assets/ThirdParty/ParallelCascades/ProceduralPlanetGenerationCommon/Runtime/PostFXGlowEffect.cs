using ParallelCascades.ProceduralPlanetGenerationCommon.PostProcessing;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime
{
    public abstract class PostFXGlowEffect : MonoBehaviour
    {
        private static readonly int s_objectPosition = Shader.PropertyToID("_Object_Position");
        protected static readonly int EffectRadius = Shader.PropertyToID("_Effect_Radius");
        protected static readonly int ObjectRadius = Shader.PropertyToID("_Object_Radius");
        protected static readonly int DensityFalloff = Shader.PropertyToID("_Density_Falloff");
        
        [SerializeField] [Min(0.001f)] protected float m_meshRadius = 0.5f;
        [SerializeField] [Range(0.1f,10)] protected float m_effectScale = 3f;
        [SerializeField] protected float m_densityFalloff = 10f;
        [SerializeField] protected Material m_material;
        [SerializeField] private RenderPassEvent m_renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
        
        private PostFXGlowPass m_postFXGlowPass;
        protected float ObjectScale;
        private bool m_initialized;

        private void OnValidate()
        {
            UpdateMaterial();
        }

        private void OnDestroy()
        {
            CleanUpPostFXGlow();
        }

        private void OnDisable()
        {
            CleanUpPostFXGlow();
            m_initialized = false;
        }

        private void OnEnable()
        {
            TryInitializePostFXGlow();
        }

        private void Update()
        {
            TryInitializePostFXGlow();
            
            SetBodyScale();
            UpdateMaterial();
        }

        private void SetBodyScale()
        {
            ObjectScale = transform.localScale.x;
        }

        protected abstract void UpdateMaterial();

        public void TryInitializePostFXGlow()
        {
            if (!m_initialized)
            {
                if (m_postFXGlowPass == null)
                {
                    m_postFXGlowPass = new PostFXGlowPass
                    {
                        renderPassEvent = m_renderPassEvent
                    };
                }

                if (m_material == null)
                {
                    return;
                }

                m_postFXGlowPass.Setup(m_material);
                m_postFXGlowPass.ConfigureInput(ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth);
                
                RenderPipelineManager.beginCameraRendering += PostFXGlowOnBeginCamera;
                m_initialized = true;
            }
        }

        private void CleanUpPostFXGlow()
        {
            RenderPipelineManager.beginCameraRendering -= PostFXGlowOnBeginCamera;
            m_initialized = false;
        }

        private void PostFXGlowOnBeginCamera(ScriptableRenderContext context, Camera cam)
        {
            
            if (cam.cameraType is not (CameraType.Game or CameraType.SceneView))
            {
                return;
            }
            
            UpdateEffectPosition(transform.position);
            cam.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(m_postFXGlowPass);
        }

        private void UpdateEffectPosition(Vector3 position)
        {
            if(m_material == null)
            {
                return;
            }
            
            m_material.SetVector(s_objectPosition, position);
        }
    }
}