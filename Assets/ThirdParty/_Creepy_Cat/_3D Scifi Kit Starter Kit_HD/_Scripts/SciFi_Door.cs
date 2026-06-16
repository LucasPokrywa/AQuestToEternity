using UnityEngine;

public class SciFi_Door : MonoBehaviour
{
    [Header("Paramètres de la porte")]
    [Tooltip("Décalage de la porte quand elle est ouverte (ex: X=2 pour horizontale, Y=3 pour verticale)")]
    public Vector3 positionOuverteOffset;
    public float vitesseOuverture = 5f;

    [Header("Sons")]
    public AudioSource sourceAudio;
    public AudioClip sonPneumatique;

    private Vector3 positionFermee;
    private Vector3 positionCible;
    private bool estOuverte = false;

    void Start()
    {
        // On mémorise la position de départ (porte fermée)
        positionFermee = transform.position;
        positionCible = positionFermee;
    }

    void Update()
    {
        // Déplace la porte de sa position actuelle vers la position cible de manière fluide (Lerp)
        transform.position = Vector3.Lerp(transform.position, positionCible, Time.deltaTime * vitesseOuverture);
    }

    // Quand le joueur entre dans la zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estOuverte = true;
            positionCible = positionFermee + positionOuverteOffset;
            JouerSon();
        }
    }

    // Quand le joueur quitte la zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estOuverte = false;
            positionCible = positionFermee;
            JouerSon();
        }
    }

    private void JouerSon()
    {
        if (sourceAudio != null && sonPneumatique != null)
        {
            sourceAudio.PlayOneShot(sonPneumatique);
        }
    }
}