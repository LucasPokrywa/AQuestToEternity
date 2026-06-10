using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ParallelCascades.Common.Rendering
{

    [ExecuteAlways]
    public class AutoLoadPipelineAsset : MonoBehaviour
    {
        public UniversalRenderPipelineAsset pipelineAsset;

        void OnEnable()
        {
            if (pipelineAsset != QualitySettings.renderPipeline)
            {
                Debug.Log($"Setting Universal Render Pipeline Asset: ({pipelineAsset.name})");
                QualitySettings.renderPipeline = pipelineAsset;
            }
        }
    }
}