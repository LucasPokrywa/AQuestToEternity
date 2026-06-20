using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class Projectile : MonoBehaviour
{
    public int damage = 1;

    [Header("Sons (SFX)")]
    [Tooltip("Le son joué au moment où le missile est tiré")]
    public AudioClip shootSound;

    [Tooltip("Le son joué quand le missile explose/touche (optionnel)")]
    public AudioClip hitSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        LizardAI lizard = collision.gameObject.GetComponent<LizardAI>();
        if (lizard != null)
        {
            lizard.TakeDamage(damage);
        }

        GolemAI golem = collision.gameObject.GetComponent<GolemAI>();
        if (golem != null)
        {
            golem.TakeDamage(damage);
        }

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        Destroy(gameObject);
    }
}