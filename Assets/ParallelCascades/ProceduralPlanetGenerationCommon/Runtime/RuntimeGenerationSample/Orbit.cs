using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGeneration.Runtime.RuntimeGenerationSample
{
    public class Orbit : MonoBehaviour
    {
        [SerializeField] private Transform m_primaryBody;

        [SerializeField] private Vector3 m_orbitAxis = Vector3.up;
        [SerializeField] private float m_orbitSpeed = 5f;
        
        [Header("Orbit Visualization")]
        [SerializeField] private bool m_showOrbitPath = true;
        [SerializeField] private bool m_useLocalSpace = true;
        [SerializeField] private int m_orbitSegments = 100;
        
        [SerializeField] private LineRenderer m_lineRenderer;
        
        public float OrbitSpeed
        {
            get => m_orbitSpeed;
            set => m_orbitSpeed = value;
        }

        private void OnValidate()
        {
            if (m_lineRenderer == null)
            {
                Debug.LogError("Assign a Line Renderer to render the orbit. This should be on a child game object of the body.");
                return;
            }
            
            UpdateOrbitPath();
        }
        
        private void UpdateOrbitPath()
        {
            Vector3 orbitCenter = m_primaryBody ? m_primaryBody.position : Vector3.zero;
            
            float orbitRadius = Vector3.Distance(transform.position,  orbitCenter);
            
            if (m_showOrbitPath && orbitRadius > 0)
            {
                GenerateOrbitPoints(orbitRadius);
            }
            else
            {
                m_lineRenderer.positionCount = 0;
            }
        }
        
        private void GenerateOrbitPoints(float radius)
        {
            m_lineRenderer.positionCount = m_orbitSegments + 1;
            m_lineRenderer.loop = true;
            m_lineRenderer.useWorldSpace = !m_useLocalSpace;
            
            Vector3 primaryPos = m_primaryBody.position;
            Vector3 orbitNormal = m_orbitAxis.normalized;
            Vector3 toObject = (transform.position - primaryPos).normalized;
            Vector3 perpendicular = Vector3.Cross(orbitNormal, toObject).normalized;
            
            for (int i = 0; i <= m_orbitSegments; i++)
            {
                float angle = (float)i / m_orbitSegments * 2f * Mathf.PI;
                Vector3 worldPoint = primaryPos + 
                    (toObject * Mathf.Cos(angle) + perpendicular * Mathf.Sin(angle)) * radius;
                
                Vector3 point = m_useLocalSpace 
                    ? m_lineRenderer.transform.InverseTransformPoint(worldPoint)
                    : worldPoint;
                    
                m_lineRenderer.SetPosition(i, point);
            }
        }

        private void Update()
        {
            if (m_primaryBody == null) return;
            
            transform.RotateAround(m_primaryBody.position, m_orbitAxis.normalized, m_orbitSpeed * Time.deltaTime);
        }
    }
}