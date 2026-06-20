using UnityEngine;

public class InteractBalise : MonoBehaviour
{
    [Header("Interaction")]
    public float interactionDistance = 3f;
    public GameObject objectToActivate;

    [Header("UI")]
    public GameObject pressEText;

    [Header("Teleport (coordinates)")]
    public Transform playerToTeleport;
    public Vector3 teleportPosition;

    private Transform playerCam;
    private bool hasInteracted = false;

    public QuestData missionRequise;

    void Start()
    {
        Camera activeCam = GetActiveCamera();

        if (activeCam != null)
            playerCam = activeCam.transform;
        else
            Debug.LogError("Aucune caméra active trouvée !");

        if (pressEText != null)
            pressEText.SetActive(false);
    }

    void Update()
    {
        if (hasInteracted || playerCam == null) return;

        float distance = Vector3.Distance(playerCam.position, transform.position);

        if (distance <= interactionDistance)
        {
            if (pressEText != null)
                pressEText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (QuestManager.Instance.IsQuestCompleted(missionRequise))
                {
                    Interact();
                }
            }
        }
        else
        {
            if (pressEText != null)
                pressEText.SetActive(false);
        }
    }

    void Interact()
    {


        hasInteracted = true;

        if (pressEText != null)
            pressEText.SetActive(false);

        // Activer objet
        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        // Téléportation par coordonnées X Y Z
        if (playerToTeleport != null)
        {
            playerToTeleport.position = new Vector3(
                teleportPosition.x,
                teleportPosition.y,
                teleportPosition.z
            );
        }


        QuestManager.Instance.ReportEvent(ObjectiveType.TalkTo, "balise");

        gameObject.SetActive(false);
    }

    Camera GetActiveCamera()
    {
        Camera[] cams = Camera.allCameras;

        foreach (Camera cam in cams)
        {
            if (cam.isActiveAndEnabled)
                return cam;
        }

        return null;
    }
}