using UnityEngine;
using UnityEngine.SceneManagement; // Uniquement si tu tiens absolument à changer de scène

public class CorvetteController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject m_PlayerGO;
    [SerializeField] private GameObject m_ShipCamera;

    [Header("UI References (Canvas Panels)")]
    [SerializeField] private GameObject m_UIManager;      // Le parent de tes menus
    [SerializeField] private GameObject m_MenuOutside;    // Menu: "Monter à bord" / "Piloter"
    [SerializeField] private GameObject m_MenuInterior;   // Menu: "Piloter" / "Sortir"
    [SerializeField] private GameObject m_MenuPiloting;   // Menu: "Quitter le siège" / "Sortir"

    [Header("Flight Settings")]
    [SerializeField] private float m_FlySpeed = 20f;
    [SerializeField] private float m_TurnSpeed = 70f;
    [SerializeField] private float m_VerticalSpeed = 20f;
    [SerializeField] private float m_LandingSpeed = 10f;
    [SerializeField] private float m_Boost = 150f;

    [Header("Interior Settings")]
    [SerializeField] private Transform m_InteriorSpawnPoint; // L'endroit où le joueur apparait dans le vaisseau

    [Header("Boutons spécifiques pour conditions")]
    [SerializeField] private GameObject m_BtnSortirInterior; // Le bouton "Sortir" du menu Intérieur
    [SerializeField] private GameObject m_BtnSortirPiloting; // Le bouton "Sortir" du menu Pilotage
    [SerializeField] private GameObject m_BtnLanding; // Le bouton "Atterrir" du menu Pilotage

    private bool m_IsPlayerNear = false;

    // États du vaisseau
    private enum ShipState { Landed, Flying, Landing }
    private ShipState m_ShipState = ShipState.Landed;

    // États du joueur par rapport au vaisseau
    private enum PlayerStatus { Outside, InInterior, Piloting }
    private PlayerStatus m_PlayerStatus = PlayerStatus.Outside;

    void Start()
    {
        m_ShipState = ShipState.Landed;

        if (m_ShipCamera != null) m_ShipCamera.SetActive(false);
        CloseAllMenus();
    }

    void Update()
    {
        // 1. GESTION DE L'INTERFACE AU CLIC SUR 'F'
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleInteractionMenu();
        }

        // 2. GESTION DU VOL ET DE L'ATTERRISSAGE
        if (m_ShipState == ShipState.Landing)
        {
            ProcessLanding();
        }
        else if (m_PlayerStatus == PlayerStatus.Piloting)
        {
            // Si on est aux commandes MAIS que le vaisseau est posé
            if (m_ShipState == ShipState.Landed)
            {
                // On attend que le joueur appuie sur Espace pour décoller
                if (Input.GetKey(KeyCode.Space))
                {
                    m_ShipState = ShipState.Flying;
                    Debug.Log("Décollage !");
                }
            }
            // Si on est aux commandes et que le vaisseau est en vol
            else if (m_ShipState == ShipState.Flying)
            {
                FlySpaceship();
            }
        }
    }

    // ----------- GESTION DES MENUS -----------

    private void ToggleInteractionMenu()
    {
        // Si un menu est déjà ouvert, on le ferme
        if (m_UIManager.activeSelf)
        {
            CloseAllMenus();
            return;
        }

        // Sinon, on ouvre le bon menu selon la position du joueur
        if (m_PlayerStatus == PlayerStatus.Outside && m_IsPlayerNear && m_ShipState == ShipState.Landed)
        {
            OpenMenu(m_MenuOutside);
        }
        else if (m_PlayerStatus == PlayerStatus.InInterior)
        {
            OpenMenu(m_MenuInterior);
        }
        else if (m_PlayerStatus == PlayerStatus.Piloting)
        {
            OpenMenu(m_MenuPiloting);
        }
    }

    private void OpenMenu(GameObject menuToOpen)
    {
        m_UIManager.SetActive(true);
        m_MenuOutside.SetActive(false);
        m_MenuInterior.SetActive(false);
        m_MenuPiloting.SetActive(false);

        // --- LOGIQUE DE CONDITION POUR LA SORTIE ---
        // On vérifie si le vaisseau est au sol
        bool peutSortir = (m_ShipState == ShipState.Landed);

        bool estEnVol = (m_ShipState == ShipState.Flying);

        // On active ou désactive les boutons de sortie selon l'état 'peutSortir'
        if (m_BtnSortirInterior != null) m_BtnSortirInterior.SetActive(peutSortir);
        if (m_BtnSortirPiloting != null) m_BtnSortirPiloting.SetActive(peutSortir);

        if (m_BtnLanding != null) m_BtnLanding.SetActive(estEnVol);
        // --------------------------------------------

        // Déverrouiller la souris pour cliquer sur l'UI (si nécessaire dans ton jeu)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        menuToOpen.SetActive(true);
    }

    private void CloseAllMenus()
    {
        m_UIManager.SetActive(false);
        m_MenuOutside.SetActive(false);
        m_MenuInterior.SetActive(false);
        m_MenuPiloting.SetActive(false);

        // Reverrouiller la souris pour le gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ----------- MÉTHODES PUBLIQUES POUR LES BOUTONS UI -----------

    // Appelé par le bouton "Monter à bord"
    public void UI_EnterInterior()
    {
        CloseAllMenus();
        m_PlayerStatus = PlayerStatus.InInterior;

        // Option A : Téléportation (Recommandé)
        if (m_InteriorSpawnPoint != null)
        {
            m_PlayerGO.transform.position = m_InteriorSpawnPoint.position;
        }

        // Option B : Chargement de scène (Moins recommandé sans Additive Loading)
        // SceneManager.LoadScene("CorvetteScene"); 

        Debug.Log("Joueur dans la zone de vie du vaisseau.");
    }

    // Appelé par le bouton "Piloter le vaisseau"
    public void UI_PilotShip()
    {
        CloseAllMenus();
        m_PlayerStatus = PlayerStatus.Piloting;

        if (m_ShipState == ShipState.Landed)

        if (m_PlayerGO != null) m_PlayerGO.SetActive(false);
        if (m_ShipCamera != null) m_ShipCamera.SetActive(true);
    }

    // Appelé par le bouton "Quitter le siège"
    public void UI_LeaveSeat()
    {
        CloseAllMenus();
        m_PlayerStatus = PlayerStatus.InInterior;

        if (m_ShipCamera != null) m_ShipCamera.SetActive(false);
        if (m_PlayerGO != null)
        {
            m_PlayerGO.SetActive(true);
            // On s'assure qu'il apparait à l'intérieur
            if (m_InteriorSpawnPoint != null) m_PlayerGO.transform.position = m_InteriorSpawnPoint.position;
        }

        Debug.Log("Le joueur quitte le siège. Le vaisseau est en pilote automatique/stationnaire.");
    }

    // Appelé par le bouton "Sortir du vaisseau"
    public void UI_ExitShip()
    {
        if (m_ShipState == ShipState.Flying || m_ShipState == ShipState.Landing)
        {
            Debug.Log("Impossible de sortir : Le vaisseau est en vol !");
            return; // Sécurité supplémentaire, même si le bouton devrait être grisé dans l'UI
        }

        CloseAllMenus();
        m_PlayerStatus = PlayerStatus.Outside;

        if (m_ShipCamera != null) m_ShipCamera.SetActive(false);
        if (m_PlayerGO != null)
        {
            Vector3 exitPosition = transform.position + transform.right * 10f;
            exitPosition.y += 0.5f;
            m_PlayerGO.transform.position = exitPosition;
            m_PlayerGO.SetActive(true);
        }

        Debug.Log("Joueur sorti du vaisseau sur la terre ferme.");
    }

    // Appelé par un bouton "Atterrir" ou via une touche (ex: appuyer sur F pendant le vol)
    public void UI_StartLanding()
    {
        if (m_ShipState == ShipState.Flying && m_PlayerStatus == PlayerStatus.Piloting)
        {
            CloseAllMenus();
            m_ShipState = ShipState.Landing;
            Debug.Log("Atterrissage automatique en cours...");
        }
    }

    // ----------- MÉCANIQUES DE VOL -----------

    private void FlySpaceship()
    {
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * vInput * m_FlySpeed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.up * hInput * m_TurnSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKey(KeyCode.Space))
            transform.Translate(Vector3.up * m_VerticalSpeed * Time.deltaTime, Space.Self);
        else if (Input.GetKey(KeyCode.LeftControl))
            transform.Translate(Vector3.down * m_VerticalSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKey(KeyCode.LeftShift))
            transform.Translate(Vector3.forward * m_Boost * Time.deltaTime, Space.Self);
    }

    private void ProcessLanding()
    {
        transform.Translate(Vector3.down * m_LandingSpeed * Time.deltaTime, Space.World);
        Ray ray = new Ray(transform.position + Vector3.up * -10f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 1.5f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                CompleteLanding();
            }
        }
    }

    private void CompleteLanding()
    {
        m_ShipState = ShipState.Landed;
        Debug.Log("Atterrissage terminé.");
        // Le joueur reste au statut "Piloting". Il devra appuyer sur F pour quitter le siège ou sortir.
    }

    // ----------- DÉTECTION DU JOUEUR -----------
    private void OnTriggerEnter(Collider other)
    {
        if (m_PlayerStatus == PlayerStatus.Outside && other.CompareTag("Player"))
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