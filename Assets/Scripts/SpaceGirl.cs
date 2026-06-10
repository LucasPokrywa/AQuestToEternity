using UnityEngine;

public class SpaceGirl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float m_MoveSpeed = 6f;
    [SerializeField] private float m_SprintMultiplier = 2f;
    [SerializeField] private float m_JumpForce = 7f;
    [SerializeField] private float m_MouseSensitivity = 2f;

    [Header("Camera & Vues")]
    [SerializeField] private Transform m_CameraPivot; // Cet objet doit être placé aux yeux/épaules
    [SerializeField] private GameObject m_CamFPS;
    [SerializeField] private GameObject m_CamTPS;

    private float m_VerticalRotation = 0f;
    private bool m_IsFirstPerson = false;

    [Header("Ball Shooting")]
    [SerializeField] GameObject m_BallPrefab;
    [SerializeField] Transform m_BallSpawnPos;
    [SerializeField] float m_BallShootSpeed = 25.0f;
    [SerializeField] Vector3 m_MissileRotationOffset;
    float m_NextShootingTime;

    [Header("Weapon Toggle")]
    [SerializeField] private GameObject m_Blaster;
    private bool m_IsArmed = false;

    Rigidbody m_Rb;
    Animator m_Animator;
    bool m_IsGrounded = true;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;

        UpdateCameraStatus();
    }

    void Update()
    {
        HandleCameraRotation();

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_IsFirstPerson = !m_IsFirstPerson;
            UpdateCameraStatus();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_IsArmed = !m_IsArmed;
            if (m_Blaster != null) m_Blaster.SetActive(m_IsArmed);
            if (m_Animator != null) m_Animator.SetBool("IsArmed", m_IsArmed);
        }

        if (m_IsArmed && Input.GetButton("Fire1") && Time.time > m_NextShootingTime)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_IsGrounded)
        {
            m_Rb.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
            m_IsGrounded = false;
        }
    }

    void HandleCameraRotation()
    {
        // 1. Rotation Horizontale (Corps)
        float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        // 2. Rotation Verticale (Calcul)
        float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity;
        m_VerticalRotation -= mouseY;
        m_VerticalRotation = Mathf.Clamp(m_VerticalRotation, -60f, 60f);

        // 3. Application directe sur les caméras
        // On ne fait plus tourner le Pivot, mais les enfants du pivot
        Quaternion targetRotation = Quaternion.Euler(m_VerticalRotation, 0f, 0f);

        if (m_CamFPS) m_CamFPS.transform.localRotation = targetRotation;
        if (m_CamTPS) m_CamTPS.transform.localRotation = targetRotation;
    }

    void UpdateCameraStatus()
    {
        if (m_CamFPS) m_CamFPS.SetActive(m_IsFirstPerson);
        if (m_CamTPS) m_CamTPS.SetActive(!m_IsFirstPerson);
    }

    void Shoot()
    {
        m_NextShootingTime = Time.time + 0.25f;

        Camera activeCam = m_IsFirstPerson ? m_CamFPS.GetComponentInChildren<Camera>() : m_CamTPS.GetComponentInChildren<Camera>();
        if (!activeCam) activeCam = Camera.main;

        Vector3 fireDirection = activeCam.transform.forward;

        Quaternion missileRotation = Quaternion.LookRotation(fireDirection) * Quaternion.Euler(m_MissileRotationOffset);
        GameObject newBallGO = Instantiate(m_BallPrefab, m_BallSpawnPos.position, missileRotation);

        Rigidbody ballRb = newBallGO.GetComponent<Rigidbody>();
        if (ballRb) ballRb.linearVelocity = fireDirection * m_BallShootSpeed;

        Destroy(newBallGO, 3f);
    }

    void FixedUpdate()
    {
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && vInput > 0;
        float currentSpeed = isSprinting ? m_MoveSpeed * m_SprintMultiplier : m_MoveSpeed;

        Vector3 moveDir = (transform.forward * vInput + transform.right * hInput).normalized;
        m_Rb.linearVelocity = new Vector3(moveDir.x * currentSpeed, m_Rb.linearVelocity.y, moveDir.z * currentSpeed);

        if (m_Animator != null)
        {
            float animSpeed = isSprinting ? Mathf.Abs(vInput) * 2f : Mathf.Abs(vInput);
            m_Animator.SetFloat("Speed", animSpeed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) m_IsGrounded = true;
    }
}