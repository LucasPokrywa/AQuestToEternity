using UnityEngine;

public class RobotAnimation : MonoBehaviour
{
    [Header("Flottaison (Toujours active)")]
    public float floatAmplitude = 0.2f; // Hauteur du rebond
    public float floatSpeed = 3f;       // Vitesse du rebond

    [Header("Mouvement Circulaire (Uniquement à l'arrêt)")]
    public float circleRadius = 0.5f;   // Taille du cercle
    public float circleSpeed = 2f;      // Vitesse de parcours du cercle
    public float spinSpeed = 50f;       // Vitesse de rotation sur lui-même

    [Header("État du Robot")]
    [Tooltip("Coche cette case quand le robot s'arrête pour déclencher le cercle.")]
    public bool isIdle = true;

    private Vector3 initialLocalPos;
    private float currentRadius = 0f;

    void Start()
    {
        // On mémorise la position de départ du visuel par rapport à son parent
        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        // 1. Calcul de la flottaison haut/bas (Axe Y)
        float newY = initialLocalPos.y + (Mathf.Sin(Time.time * floatSpeed) * floatAmplitude);

        // 2. Transition fluide pour le rayon du cercle (évite un "téléport" brutal quand il s'arrête)
        float targetRadius = isIdle ? circleRadius : 0f;
        currentRadius = Mathf.Lerp(currentRadius, targetRadius, Time.deltaTime * 5f);

        // 3. Calcul du cercle sur les axes X et Z
        float newX = initialLocalPos.x + (Mathf.Cos(Time.time * circleSpeed) * currentRadius);
        float newZ = initialLocalPos.z + (Mathf.Sin(Time.time * circleSpeed) * currentRadius);

        // On applique les nouvelles coordonnées (position locale pour ne pas gêner le parent)
        transform.localPosition = new Vector3(newX, newY, newZ);

        // 4. Rotation sur lui-même si le robot est à l'arrêt
        if (isIdle)
        {
            // Tourne autour de son axe Y local
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
        }
    }
}