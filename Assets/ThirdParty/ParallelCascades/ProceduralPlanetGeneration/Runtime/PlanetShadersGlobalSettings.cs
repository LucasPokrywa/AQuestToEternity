using System;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGeneration.Runtime
{
    [ExecuteInEditMode]
    public class PlanetShadersGlobalSettings : MonoBehaviour
    {
        private static readonly int s_orthographicCamera = Shader.PropertyToID("_Orthographic_Camera");
        private static readonly int s_orthographicCameraSize = Shader.PropertyToID("_Orthographic_Camera_Size");

        [SerializeField] private bool m_omnidirectionalSpaceLighting;

        [SerializeField] private Camera m_camera;

        private void OnValidate()
        {
            if (m_omnidirectionalSpaceLighting)
            {
                Shader.EnableKeyword("_OMNIDIRECTIONAL_SPACE_LIGHTING");
            }
            else
            {
                Shader.DisableKeyword("_OMNIDIRECTIONAL_SPACE_LIGHTING");
            }
        }

        private void Update()
        {
            if (m_camera != null)
            {
                Shader.SetGlobalInteger(s_orthographicCamera, m_camera.orthographic ? 1 : 0);
                Shader.SetGlobalFloat(s_orthographicCameraSize, m_camera.orthographicSize);
            }
        }
    }
}