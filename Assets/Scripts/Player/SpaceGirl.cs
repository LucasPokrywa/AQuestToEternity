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

    [Header("Camera TPS - Orbite verticale")]
    [Tooltip("Distance entre la caméra TPS et le personnage")]
    [SerializeField] private float m_TPSDistance = 3f;
    [Tooltip("Vitesse de lissage du mouvement de la caméra (plus haut = plus rapide/raide)")]
    [SerializeField] private float m_TPSFollowSmoothness = 12f;
    [Tooltip("Décalage du point visé par la caméra par rapport au pivot (Y positif = remonte le cadrage)")]
    [SerializeField] private Vector3 m_TPSLookOffset = new Vector3(0f, 0.5f, 0f);

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

    private bool isJump = false;

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
            isJump = true;
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

        Quaternion targetRotation = Quaternion.Euler(m_VerticalRotation, 0f, 0f);

        // 3a. Cam FPS : reste à hauteur des yeux, seule la rotation locale change
        if (m_CamFPS) m_CamFPS.transform.localRotation = targetRotation;

        // 3b. Cam TPS : orbite verticalement autour du point visé (= le personnage + offset)
        //     afin de toujours rester centrée sur lui, qu'on regarde en haut ou en bas
        if (m_CamTPS && m_CameraPivot)
        {
            // Point réel visé par la caméra : le pivot décalé par m_TPSLookOffset
            Vector3 lookTarget = m_CameraPivot.position + m_TPSLookOffset;

            // Rotation complète = rotation horizontale du corps + rotation verticale de la souris
            Quaternion fullRotation = m_CameraPivot.rotation * targetRotation;

            // Position désirée : sur un arc de cercle autour du point visé, à distance fixe
            Vector3 desiredPosition = lookTarget - (fullRotation * Vector3.forward * m_TPSDistance);

            // Lissage du déplacement pour éviter les à-coups
            m_CamTPS.transform.position = Vector3.Lerp(
                m_CamTPS.transform.position,
                desiredPosition,
                Time.deltaTime * m_TPSFollowSmoothness
            );

            // On regarde toujours vers le même point : le personnage reste centré à l'écran
            m_CamTPS.transform.LookAt(lookTarget);
        }
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
