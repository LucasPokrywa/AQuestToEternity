using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace ParallelCascades.Common.Runtime
{
    public class FlyCamera : MonoBehaviour
    {
        public float MaxVAngle = 89;
        public float MinVAngle = -89;
    
        public float FlyRotationSpeed = 1.5f;
        public float FlyRotationSharpness = 999999;
        public float FlyMoveSharpness = 10;
        public float FlyMaxMoveSpeed = 35;
        public float FlySprintSpeedBoost = 5;

        private Vector3 m_currentMoveVelocity;
        private float m_pitchAngle;
        private Vector3 m_planarForward = Vector3.forward;

        private bool m_ignoreInput;
        
        private struct CameraInputs
        {
            public Vector3 Move;
            public Vector2 Look;
            public bool Sprint;
        }
        
        void Start()
        {
            var cameraTransform = Camera.main.transform;
            Vector3 eulerAngles = cameraTransform.rotation.eulerAngles;
            
            m_pitchAngle = eulerAngles.x;
    
            // Make sure we retain whatever rotation camera had on start.
            m_planarForward = Quaternion.Euler(0f, eulerAngles.y, 0f) * Vector3.forward;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            m_ignoreInput = false;
        }
        
        void Update()
        {
            bool escapePressed = false;
#if ENABLE_INPUT_SYSTEM
            var kb = Keyboard.current;
            if (kb != null)
            {
                escapePressed = kb.escapeKey.wasPressedThisFrame;
            }
#else
            escapePressed = Input.GetKeyDown(KeyCode.Escape);
#endif
            if (escapePressed)
            {
                // Toggle pause / pointer mode
                m_ignoreInput = !m_ignoreInput;
                if (m_ignoreInput)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }

            CameraInputs cameraInputs = new CameraInputs();

#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            var mouse = Mouse.current;
            
            if (keyboard == null || mouse == null)
            {
                return;
            }
            
            cameraInputs.Move = new Vector3(
                (keyboard.dKey.isPressed ? 1f : 0f) + (keyboard.aKey.isPressed ? -1f : 0f),
                (keyboard.eKey.isPressed ? 1f : 0f) + (keyboard.qKey.isPressed ? -1f : 0f),
                (keyboard.wKey.isPressed ? 1f : 0f) + (keyboard.sKey.isPressed ? -1f : 0f));
            cameraInputs.Look = new Vector2(
                mouse.delta.x.ReadValue() * 0.05f,
                mouse.delta.y.ReadValue() * 0.05f);
            cameraInputs.Sprint = keyboard.leftShiftKey.isPressed;
#elif ENABLE_LEGACY_INPUT_MANAGER
            cameraInputs.Move = new Vector3(
                (Input.GetKey(KeyCode.D) ? 1f : 0f) + (Input.GetKey(KeyCode.A) ? -1f : 0f),
                (Input.GetKey(KeyCode.E) ? 1f : 0f) + (Input.GetKey(KeyCode.Q) ? -1f : 0f),
                (Input.GetKey(KeyCode.W) ? 1f : 0f) + (Input.GetKey(KeyCode.S) ? -1f : 0f));
            cameraInputs.Look = new Vector2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y"));
            cameraInputs.Sprint = Input.GetKey(KeyCode.LeftShift);
#endif

            cameraInputs.Move = cameraInputs.Move *
                                Mathf.Clamp(Vector3.Magnitude(cameraInputs.Move),0,1); // Clamp move inputs magnitude

            if (m_ignoreInput)
            { 
                // Camera paused while pointer is free.
                return;
            }
        
            // Yaw
            float yawAngleChange = cameraInputs.Look.x * FlyRotationSpeed;
            Quaternion yawRotation = Quaternion.Euler(Vector3.up *  yawAngleChange);

            m_planarForward = yawRotation * m_planarForward;

            // Pitch
            m_pitchAngle += -cameraInputs.Look.y * FlyRotationSpeed;
            m_pitchAngle = Mathf.Clamp(m_pitchAngle, MinVAngle,
                MaxVAngle);
            Quaternion pitchRotation = Quaternion.Euler(Vector3.right *  m_pitchAngle);

            // Final rotation
            Quaternion targetRotation =
                Quaternion.LookRotation(m_planarForward, Vector3.up) * pitchRotation;
        
            float deltaTime = Time.deltaTime;
        
            var cameraTransform = UnityEngine.Camera.main.transform;
        
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation,
                GetSharpnessInterpolant(FlyRotationSharpness, deltaTime));

            // Move
            Vector3 worldMoveInputs = cameraTransform.rotation * cameraInputs.Move;
            float finalMaxSpeed = FlyMaxMoveSpeed;
            if (cameraInputs.Sprint)
            {
                finalMaxSpeed *= FlySprintSpeedBoost;
            }

            m_currentMoveVelocity = Vector3.Lerp(m_currentMoveVelocity,
                worldMoveInputs * finalMaxSpeed,
                GetSharpnessInterpolant(FlyMoveSharpness, deltaTime));
            cameraTransform.position += m_currentMoveVelocity * deltaTime;
        }
        
        private static float GetSharpnessInterpolant(float sharpness, float dt)
        {
            return Mathf.Clamp(1f - Mathf.Exp(-sharpness * dt),0,1);
        }
    }
}