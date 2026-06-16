using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject m_PlayerGO;
    [SerializeField] private GameObject m_ShipCamera;

    [Header("Flight Settings")]
    [SerializeField] private float m_FlySpeed = 20f;
    [SerializeField] private float m_TurnSpeed = 70f;
    [SerializeField] private float m_VerticalSpeed = 20f;   // Vitesse pour monter/descendre
    [SerializeField] private float m_LandingSpeed = 10f;    // Vitesse de la descente automatique
    [SerializeField] private float m_Boost = 150f;          // Vitesse du boost quand le vaisseau speed

    private bool m_IsPlayerNear = false;

    // Définition des différents états possibles du vaisseau
    private enum ShipState { Empty, Flying, Landing }
    private ShipState m_CurrentState = ShipState.Empty;

    void Start()
    {
        if (m_ShipCamera != null) m_ShipCamera.SetActive(false);
    }

    void Update()
    {
        // On agit différemment selon l'état actuel du vaisseau
        switch (m_CurrentState)
        {
            case ShipState.Empty:
                // Monter dans le vaisseau
                if (m_IsPlayerNear && Input.GetKeyDown(KeyCode.F))
                {
                    BoardSpaceship();
                }
                break;

            case ShipState.Flying:
                // Piloter le vaisseau
                FlySpaceship();

                // Enclencher l'atterrissage
                if (Input.GetKeyDown(KeyCode.F))
                {
                    StartLanding();
                }
                break;

            case ShipState.Landing:
                // Phase d'atterrissage automatique
                ProcessLanding();
                break;
        }
    }

    private void BoardSpaceship()
    {
        m_CurrentState = ShipState.Flying;

        if (m_PlayerGO != null) m_PlayerGO.SetActive(false);
        if (m_ShipCamera != null) m_ShipCamera.SetActive(true);

        Debug.Log("Décollage imminent !");
    }

    private void FlySpaceship()
    {
        float vInput = Input.GetAxis("Vertical");   // Z / S (Avancer/Reculer)
        float hInput = Input.GetAxis("Horizontal"); // Q / D (Tourner)

        transform.Translate(Vector3.forward * vInput * m_FlySpeed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.up * hInput * m_TurnSpeed * Time.deltaTime, Space.Self);

        // Vol Vertical : Espace pour monter, Maj Gauche (Left Shift) pour descendre
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * m_VerticalSpeed * Time.deltaTime, Space.Self);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(Vector3.down * m_VerticalSpeed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.forward * m_Boost * Time.deltaTime, Space.Self);
        }
    }

    private void StartLanding()
    {
        m_CurrentState = ShipState.Landing;
        Debug.Log("Atterrissage automatique en cours...");
    }

    private void ProcessLanding()
    {
        // On fait descendre le vaisseau vers le bas (par rapport au monde, pas au vaisseau)
        transform.Translate(Vector3.down * m_LandingSpeed * Time.deltaTime, Space.World);

        // On lance un rayon (Raycast) vers le bas pour détecter le sol.
        // On part d'un peu plus haut que le centre du vaisseau pour être sûr de bien détecter.
        // QueryTriggerInteraction.Ignore empêche le rayon de s'arrêter sur la zone d'interaction du vaisseau.
        Ray ray = new Ray(transform.position + Vector3.up * 1f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 1.5f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            // Si le rayon touche un objet avec le tag "Ground" à moins de 2.5 mètres
            if (hit.collider.CompareTag("Ground"))
            {
                CompleteLanding();
            }
        }
    }

    private void CompleteLanding()
    {
        m_CurrentState = ShipState.Empty;

        // Éteindre la caméra du vaisseau
        if (m_ShipCamera != null) m_ShipCamera.SetActive(false);

        // Faire réapparaître le joueur
        if (m_PlayerGO != null)
        {
            // On positionne le joueur à 3 mètres sur la droite du vaisseau
            Vector3 exitPosition = transform.position + transform.right * 10f;

            // On s'assure qu'il n'est pas enfoncé dans le sol
            exitPosition.y += 0.5f;

            m_PlayerGO.transform.position = exitPosition;
            m_PlayerGO.SetActive(true);
        }

        Debug.Log("Atterrissage terminé. Joueur sorti.");
    }

    // ----------- DÉTECTION DU JOUEUR -----------
    private void OnTriggerEnter(Collider other)
    {
        if (m_CurrentState == ShipState.Empty && other.CompareTag("Player"))
        {
            m_IsPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_IsPlayerNear = false;
        }
    }
}