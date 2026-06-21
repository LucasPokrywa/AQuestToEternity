using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 80f;
    public float rotationSpeed = 70f;
    public float verticalSpeed = 50f;

    [Header("Planet Blocking Triggers")]
    public Collider[] blockingTriggers;
    public float shipRadius = 1f;

    [Header("Engine Trails")]
    public TrailRenderer leftTrail;
    public TrailRenderer rightTrail;

    [Header("Transition vers la base (Sortie)")]
    [Tooltip("L'endroit où la SpaceGirl apparaît dans la base")]
    public Transform m_InteriorSpawnPoint;

    [Tooltip("Le GameObject de la SpaceGirl")]
    public GameObject playerCharacter;

    [Tooltip("La caméra externe du vaisseau")]
    public GameObject shipCamera;

    // --- NOUVEAU : Référence à l'environnement de la base ---
    [Header("Environnement de la Base")]
    [Tooltip("Glissez ici le GameObject parent qui contient tous les murs/objets de votre base")]
    public GameObject shipInteriorBase;

    [Header("Éléments à désactiver à pied")]
    [Tooltip("Glissez ici les scripts du vaisseau (ex: tir, radar) à désactiver")]
    public MonoBehaviour[] shipScriptsToDisable;

    [Tooltip("Glissez ici les Canvas/UI du vaisseau à cacher")]
    public GameObject[] shipUIToHide;

    private bool isFlying = true;

    // Tableaux de mémoire pour retenir l'état précédent
    private bool[] uiStates;

    void Start()
    {
        // Initialisation de la mémoire en fonction du nombre d'UI
        if (shipUIToHide != null)
        {
            uiStates = new bool[shipUIToHide.Length];
        }

        // --- NOUVEAU : On s'assure que la base est bien éteinte au lancement (vu qu'on commence en vol) ---
        if (shipInteriorBase != null)
        {
            shipInteriorBase.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetTrails(false);
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        // --- BASCULE DU SIÈGE (Touche F) ---
        if (keyboard.fKey.wasPressedThisFrame)
        {
            if (isFlying)
            {
                LeaveSeat();
            }
            else
            {
                ResumePiloting();
            }
            return;
        }

        // --- VOL ---
        if (isFlying)
        {
            FlySpaceship(keyboard);
        }
    }

    private void FlySpaceship(Keyboard keyboard)
    {
        float moveForward = 0f;
        float turn = 0f;
        float moveUp = 0f;

        if (keyboard.zKey.isPressed || keyboard.wKey.isPressed) moveForward = 1f;
        if (keyboard.sKey.isPressed) moveForward = -1f;
        if (keyboard.qKey.isPressed || keyboard.aKey.isPressed) turn = -1f;
        if (keyboard.dKey.isPressed) turn = 1f;
        if (keyboard.spaceKey.isPressed) moveUp = 1f;
        if (keyboard.leftCtrlKey.isPressed) moveUp = -1f;

        SetTrails(moveForward > 0f);

        transform.Rotate(Vector3.up * turn * rotationSpeed * Time.deltaTime);

        Vector3 movement =
            transform.forward * moveForward * moveSpeed * Time.deltaTime +
            transform.up * moveUp * verticalSpeed * Time.deltaTime;

        Vector3 nextPosition = transform.position + movement;

        if (!WouldEnterBlockingTrigger(nextPosition))
        {
            transform.position = nextPosition;
        }
    }

    private void LeaveSeat()
    {
        isFlying = false;
        SetTrails(false);

        // --- NOUVEAU : Activer l'environnement de la base ---
        if (shipInteriorBase != null)
        {
            shipInteriorBase.SetActive(true);
        }

        // 1. Placer et activer le joueur dans la base
        if (m_InteriorSpawnPoint != null && playerCharacter != null)
        {
            playerCharacter.transform.position = m_InteriorSpawnPoint.position;
            playerCharacter.transform.rotation = m_InteriorSpawnPoint.rotation;
            playerCharacter.SetActive(true);
        }

        // 2. Désactiver la caméra du vaisseau
        if (shipCamera != null) shipCamera.SetActive(false);

        // 3. Désactiver les autres scripts
        for (int i = 0; i < shipScriptsToDisable.Length; i++)
        {
            if (shipScriptsToDisable[i] != null)
                shipScriptsToDisable[i].enabled = false;
        }

        // 4. MÉMORISER ET CACHER l'UI du vaisseau
        for (int i = 0; i < shipUIToHide.Length; i++)
        {
            if (shipUIToHide[i] != null)
            {
                uiStates[i] = shipUIToHide[i].activeSelf;
                shipUIToHide[i].SetActive(false);
            }
        }

        Debug.Log("Sortie du siège : Joueur dans la base. Base affichée.");
    }

    private void ResumePiloting()
    {
        isFlying = true;

        // 1. Désactiver la SpaceGirl
        if (playerCharacter != null) playerCharacter.SetActive(false);

        // --- NOUVEAU : Désactiver l'environnement de la base pour libérer de la mémoire et la cacher ---
        if (shipInteriorBase != null)
        {
            shipInteriorBase.SetActive(false);
        }

        // 2. Réactiver la caméra du vaisseau
        if (shipCamera != null) shipCamera.SetActive(true);

        // 3. Réactiver les scripts (tirs, armes)
        for (int i = 0; i < shipScriptsToDisable.Length; i++)
        {
            if (shipScriptsToDisable[i] != null)
                shipScriptsToDisable[i].enabled = true;
        }

        // 4. RESTAURER la mémoire de l'UI du vaisseau
        for (int i = 0; i < shipUIToHide.Length; i++)
        {
            if (shipUIToHide[i] != null)
            {
                shipUIToHide[i].SetActive(uiStates[i]);
            }
        }

        Debug.Log("Reprise du contrôle du vaisseau. Base masquée.");
    }

    private void SetTrails(bool active)
    {
        if (leftTrail != null) leftTrail.emitting = active;
        if (rightTrail != null) rightTrail.emitting = active;
    }

    private bool WouldEnterBlockingTrigger(Vector3 nextPosition)
    {
        if (blockingTriggers == null || blockingTriggers.Length == 0) return false;

        foreach (Collider trigger in blockingTriggers)
        {
            if (trigger == null) continue;
            Vector3 closestPoint = trigger.ClosestPoint(nextPosition);
            if (Vector3.Distance(nextPosition, closestPoint) < shipRadius) return true;
        }
        return false;
    }
}