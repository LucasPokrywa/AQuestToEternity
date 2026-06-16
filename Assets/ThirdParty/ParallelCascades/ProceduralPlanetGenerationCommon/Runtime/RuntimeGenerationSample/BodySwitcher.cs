using System;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime.RuntimeGenerationSample
{
    public class BodySwitcher : MonoBehaviour
    {
        [Serializable]
        private struct SwitchableBody
        {
            public ProceduralBodyBase[] Bodies;
        }
        
        [SerializeField] private SwitchableBody[] m_bodies;
        
        private SwitchableBody m_activeBody;

        private void Awake()
        {
            m_activeBody = m_bodies[0];
        }

        public void OnBodyChanged(int index)
        {
            
            for (int i = 0; i < m_bodies.Length; i++)
            {
                foreach (var body in m_bodies[i].Bodies)
                {
                    body.gameObject.SetActive(i == index);
                }
            }
            
            m_activeBody = m_bodies[index];
        }

        public void GenerateBody()
        {
            foreach(var body in m_activeBody.Bodies)
            {
                body.RandomizeEntirePlanet();
            }
        }
    }
}