using System;
using ParallelCascades.Common.Runtime;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGeneration.Runtime.RuntimeGenerationSample
{
    public class SimulationSpeedManager : MonoBehaviour
    {
        private struct OrbitData
        {
            public Orbit OrbitManager;
            public RotatingBody RotatingBodyManager;
            public float InitialOrbitSpeed;
        }
        
        private OrbitData[] m_orbitDatas;

        private void Awake()
        {
            var orbitManagers = FindObjectsByType<Orbit>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            m_orbitDatas = new OrbitData[orbitManagers.Length];
            
            for (int i = 0; i < orbitManagers.Length; i++)
            {
                m_orbitDatas[i] = new OrbitData
                {
                    OrbitManager = orbitManagers[i],
                    RotatingBodyManager = orbitManagers[i].GetComponent<RotatingBody>(),
                    InitialOrbitSpeed = orbitManagers[i].OrbitSpeed,
                };
            }
        }

        public void SetSimulationSpeed(float speedMultiplier)
        {
            foreach (var orbitData in m_orbitDatas)
            {
                orbitData.OrbitManager.OrbitSpeed = speedMultiplier * orbitData.InitialOrbitSpeed;
                if (orbitData.RotatingBodyManager != null)
                {
                    orbitData.RotatingBodyManager.ModifyInitialSpeed(speedMultiplier);
                }
            }
        }
    }
}