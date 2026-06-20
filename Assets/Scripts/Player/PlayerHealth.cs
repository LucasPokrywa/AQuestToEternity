using UnityEngine;
using UnityEngine.UI;
using System.Collections; // REQUIS pour utiliser les Coroutines (délais)

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;

    [Header("UI Bar")]
    public Slider healthBar;
    public Image fillImage;

    [Header("Game Over")]
    public GameObject deathCanvas;
    [Tooltip("Temps d'attente (en secondes) avant d'afficher l'écran de texte, pour laisser l'animation de mort se jouer.")]
    [SerializeField] private float delayBeforeDeathScreen = 2f;

    private int currentHealth;
    private Color startingColor;
    private Animator anim;        // Référence automatique à l'Animator du joueur
    private bool isDead = false;  // Évite de déclencher la mort plusieurs fois si on reprend des dégâts

    void Awake()
    {
        currentHealth = maxHealth;

        // On récupère automatiquement l'Animator présent sur le joueur
        anim = GetComponent<Animator>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (fillImage != null)
        {
            startingColor = fillImage.color;
        }

        if (deathCanvas != null)
        {
            deathCanvas.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Si le joueur est déjà mort, on ignore les dégâts suivants

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (fillImage != null)
        {
            if (currentHealth <= (maxHealth * 0.3f))
            {
                fillImage.color = Color.red;
            }
            else
            {
                fillImage.color = startingColor;
            }
        }

        Debug.Log("Vie restante : " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Le joueur a succombé à ses blessures.");

        // 1. Déclencher l'animation de mort dans l'Animator
        if (anim != null)
        {
            anim.SetBool("IsDead", true);
        }

        // 2. Désactiver les mouvements immédiatement pour que SpaceGirl s'arrête net
        SpaceGirl spaceGirlScript = GetComponent<SpaceGirl>();
        if (spaceGirlScript != null)
        {
            spaceGirlScript.enabled = false;
        }

        // Bloquer la physique (vitesse) du Rigidbody pour éviter qu'elle ne continue à glisser
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            // rb.isKinematic = true; // Optionnel : cochez ceci si vous voulez qu'elle traverse le sol ou ne subisse plus aucune force physique
        }

        // 3. Lancer la séquence d'affichage différé du Canvas
        StartCoroutine(ShowDeathScreenSequence());
    }

    // Cette fonction s'exécute en parallèle et gère le timing
    private IEnumerator ShowDeathScreenSequence()
    {
        // On attend le nombre de secondes défini dans l'inspecteur
        yield return new WaitForSeconds(delayBeforeDeathScreen);

        // Une fois le temps écoulé, on affiche le Canvas de mort
        if (deathCanvas != null)
        {
            deathCanvas.SetActive(true);
        }

        // Libérer la souris pour pouvoir interagir avec l'UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}