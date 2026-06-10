using UnityEngine;

public class PlayerRobotLink : MonoBehaviour
{
    [Header("Référence au script du Robot")]
    // La référence vers le script d'animation qu'on a créé tout à l'heure
    public RobotAnimation robotAnimScript;

    private Vector3 lastPosition;

    void Start()
    {
        // On mémorise la position de départ
        lastPosition = transform.position;
    }

    void Update()
    {
        // 1. Calcul de la distance parcourue depuis la dernière frame
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        // 2. Calcul de la vitesse (distance / temps)
        float currentSpeed = distanceMoved / Time.deltaTime;

        // 3. Détermination de l'état (vitesse très faible = à l'arrêt)
        // On utilise 0.05f pour avoir une petite marge de tolérance
        bool playerIsStopped = currentSpeed < 0.05f;

        // 4. On transmet l'état au script du robot
        if (robotAnimScript != null)
        {
            robotAnimScript.isIdle = playerIsStopped;
        }

        // 5. On met à jour l'ancienne position pour la frame suivante
        lastPosition = transform.position;
    }
}