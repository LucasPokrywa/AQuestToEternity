using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGeneration.Runtime.CustomLighting
{
    [ExecuteInEditMode]
    public class MainSpaceLight : MonoBehaviour
    {
        private static readonly int s_mainSpaceLightPosition = Shader.PropertyToID("_Main_Space_Light_Position");
        private static readonly int s_mainSpaceLightColor = Shader.PropertyToID("_Main_Space_Light_Color");

        [SerializeField] private Color m_color;

        private void Update()
        {
            Shader.SetGlobalVector(s_mainSpaceLightPosition, transform.position);
            Shader.SetGlobalColor(s_mainSpaceLightColor, m_color);
        }
    }
}