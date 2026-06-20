using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AsteroidDestroyer : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public MissionManager missionManager;

    [Header("UI")]
    [SerializeField] private GameObject promptText;
    [SerializeField] private GameObject promptPanel;

    [Header("Collection")]
    public float interactionRange = 5f;

    private AsteroidInteractable currentTarget;

    void Start()
    {
        if (player == null)
            player = transform;

        if (promptText != null)
            promptText.SetActive(false);

        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    void Update()
    {
        CheckNearbyAsteroid();

        bool ePressed =
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame;

        if (currentTarget != null && ePressed)
        {


            QuestManager.Instance.ReportEvent(ObjectiveType.Collect, "asteroides");

            Destroy(currentTarget.gameObject);

            currentTarget = null;

            if (promptText != null)
                promptText.SetActive(false);

            if (promptPanel != null)
                promptPanel.SetActive(false);
        }
    }

    void CheckNearbyAsteroid()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                player.position,
                interactionRange
            );

        AsteroidInteractable closest = null;
        float closestDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            AsteroidInteractable asteroid =
                hit.GetComponentInParent<AsteroidInteractable>();

            if (asteroid != null)
            {
                float distance =
                    Vector3.Distance(
                        player.position,
                        asteroid.transform.position
                    );

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = asteroid;
                }
            }
        }

        currentTarget = closest;

        if (currentTarget != null)
        {
            if (promptText != null)
            {
                promptText.SetActive(true);

                Text textComponent =
                    promptText.GetComponent<Text>();

                if (textComponent != null)
                {
                    textComponent.text =
                        "Appuie sur E pour collecter l'astéroïde";
                }
            }

            if (promptPanel != null)
                promptPanel.SetActive(true);
        }
        else
        {
            if (promptText != null)
                promptText.SetActive(false);

            if (promptPanel != null)
                promptPanel.SetActive(false);
        }
    }
}