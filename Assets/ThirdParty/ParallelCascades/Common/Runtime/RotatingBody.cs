using System;
using UnityEngine;

namespace ParallelCascades.Common.Runtime
{
    public class RotatingBody : MonoBehaviour
    {
        [Tooltip("The value is in degrees per second - x, y and z Euler rotation.")]
        [SerializeField] private Vector3 m_RotationSpeedPerAxis;

        private Vector3 m_InitialSpeed;

        private int m_DirectionSign = 1;

        private void Awake()
        {
            m_InitialSpeed = m_RotationSpeedPerAxis;
        }

        private void Update()
        {
            if (m_RotationSpeedPerAxis != Vector3.zero)
            {
                Quaternion rotation = Quaternion.Euler(m_RotationSpeedPerAxis * Time.deltaTime);
                
                transform.localRotation *= rotation;
            }
        }

        public void ModifyInitialSpeed(float modifier)
        {
            m_RotationSpeedPerAxis = m_InitialSpeed * m_DirectionSign * modifier;
        }

        public void FlipDirection()
        {
            m_DirectionSign = -m_DirectionSign;
            m_RotationSpeedPerAxis = -m_RotationSpeedPerAxis;
        }
    }
}