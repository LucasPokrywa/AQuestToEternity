using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))] // Ajout automatique de l'AudioSource pour le son
public class GolemAI : MonoBehaviour // NOM DE LA CLASSE CORRIGÉ ICI
{
    [Header("Paramètres de l'ennemi")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Tooltip("Dégâts infligés au joueur par attaque")]
    public int attackDamage = 15; // Ajout des dégâts d'attaque

    [Header("Détection et Attaque")]
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    [Header("Sons (SFX)")]
    public AudioClip rageSound; // Glissez votre fichier audio de cri ici
    private AudioSource audioSource;

    private Animator anim;
    private NavMeshAgent agent;

    // États internes de l'ennemi
    private bool isSleeping = true;
    private bool isWakingUp = false;
    private bool isDead = false;
    private float lastAttackTime = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>(); // Initialisation du lecteur de son

        currentHealth = maxHealth;

        // Configuration initiale : l'ennemi s'endort
        anim.SetTrigger("SleepStart");
        agent.isStopped = true;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si l'ennemi dort et que le joueur approche
        if (isSleeping && !isWakingUp)
        {
            if (distanceToPlayer <= detectionRange)
            {
                StartCoroutine(WakeUpSequence());
            }
        }
        // Si l'ennemi est réveillé et prêt à agir
        else if (!isSleeping && !isWakingUp)
        {
            ComportementCombat(distanceToPlayer);
        }
    }

    private IEnumerator WakeUpSequence()
    {
        isWakingUp = true;

        // 1. Fin du sommeil
        anim.SetTrigger("SleepEnd");
        yield return new WaitForSeconds(1.5f); // Ajustez selon la durée de votre animation SleepEnd

        // 2. L'ennemi s'énerve
        anim.SetTrigger("Rage");

        // --- NOUVEAU : JOUER LE SON DE RAGE ---
        if (rageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(rageSound);
        }

        yield return new WaitForSeconds(1.5f); // Ajustez selon la durée de l'animation Rage

        // 3. Prêt à attaquer
        isSleeping = false;
        isWakingUp = false;
        agent.isStopped = false; // Autorise le déplacement
    }

    private void ComportementCombat(float distanceToPlayer)
    {
        // Regarder le joueur
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (distanceToPlayer > attackRange)
        {
            // Le joueur est loin -> Courir/Marcher vers lui
            agent.isStopped = false;
            agent.SetDestination(player.position);

            // On met Walk à 1 (ou plus selon votre BlendTree/Transition)
            anim.SetFloat("Walk", agent.velocity.magnitude);
        }
        else
        {
            // Le joueur est à portée -> Arrêt et Attaque
            agent.isStopped = true;
            anim.SetFloat("Walk", 0f);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attaquer();
            }
        }
    }

    private void Attaquer()
    {
        lastAttackTime = Time.time;
        anim.SetTrigger("Attack1");

        // --- NOUVEAU : INFLIGER DES DÉGÂTS AU JOUEUR ---
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    // Fonction à appeler quand le joueur frappe l'ennemi
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Mourir();
        }
        else
        {
            // Déclenche l'animation de coup reçu
            anim.SetTrigger("Hit");

            // Optionnel : Stopper l'agent brièvement pendant le hit
            StartCoroutine(StunBriefly());
        }
    }

    private IEnumerator StunBriefly()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(0.5f); // Temps de l'animation Hit
        if (!isDead && !isSleeping) agent.isStopped = false;
    }

    private void Mourir()
    {
        isDead = true;
        agent.isStopped = true;
        anim.SetFloat("Walk", 0f);

        // Déclenche l'animation de mort
        anim.SetTrigger("Die");

        QuestManager.Instance.ReportEvent(ObjectiveType.Kill, "golem");

        // Optionnel : Détruire l'objet après un certain temps
        Destroy(gameObject, 5f);
    }
}