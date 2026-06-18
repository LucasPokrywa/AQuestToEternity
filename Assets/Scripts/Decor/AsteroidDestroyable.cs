using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AsteroidDestroyer : MonoBehaviour
{
    public Transform player;
    public float interactionRange = 5f;
    public Text promptText;

    private AsteroidInteractable currentTarget;

    void Start()
    {
        if (player == null) player = transform;
        if (promptText != null) promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckNearbyAsteroid();

        bool ePressed = Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;

        if (currentTarget != null && ePressed)
        {
            Destroy(currentTarget.gameObject);
            currentTarget = null;
            if (promptText != null) promptText.gameObject.SetActive(false);
        }
    }

    void CheckNearbyAsteroid()
    {
        Collider[] hits = Physics.OverlapSphere(player.position, interactionRange);

        AsteroidInteractable closest = null;
        float closestDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            AsteroidInteractable asteroid = hit.GetComponentInParent<AsteroidInteractable>();
            if (asteroid != null)
            {
                float distance = Vector3.Distance(player.position, asteroid.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = asteroid;
                }
            }
        }

        currentTarget = closest;

        if (promptText != null)
        {
            if (currentTarget != null)
            {
                promptText.text = "Appuyez sur E pour interagir";
                promptText.gameObject.SetActive(true);
            }
            else
            {
                promptText.gameObject.SetActive(false);
            }
        }
    }
}