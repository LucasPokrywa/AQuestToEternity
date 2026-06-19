using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1;

    void OnCollisionEnter(Collision collision)
    {
        // On vérifie si l'objet touché possède le script LizardAI
        LizardAI lizard = collision.gameObject.GetComponent<LizardAI>();

        if (lizard != null)
        {
            lizard.TakeDamage(damage);
        }

        // Optionnel : Détruire le missile après l'impact
        Destroy(gameObject);
    }
}