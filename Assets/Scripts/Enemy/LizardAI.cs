using UnityEngine;
using UnityEngine.AI;

public class LizardAI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Health")]
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 12f;
    [SerializeField] private float attackRange = 2f;

    [Header("Combat")]
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private int damage = 10;

    private float nextAttackTime;

    private Animator animator;
    private NavMeshAgent agent;

    private bool isDead;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        currentHealth = maxHealth;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (isDead)
            return;

        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRange)
        {
            Idle();
        }
        else if (distance > attackRange)
        {
            Chase();
        }
        else
        {
            Attack();
        }
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.SetFloat("Speed", 0f); // Correspond au paramètre Float de ton Animator
        animator.SetBool("InBattle", false); // Correspond au paramètre Bool de ton Animator
    }

    void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetFloat("Speed", 1f); // Modifie la valeur selon ce qui déclenche ta transition vers "run"
        animator.SetBool("InBattle", false);
    }

    void Attack()
    {
        agent.isStopped = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        animator.SetFloat("Speed", 0f);
        animator.SetBool("InBattle", true);

        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + attackCooldown;

        if (Random.value > 0.5f) animator.SetTrigger("Attack1");
        else animator.SetTrigger("Attack2");
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead)
            return;

        currentHealth -= damageAmount;

        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        agent.isStopped = true;
        agent.enabled = false;

        animator.SetTrigger("Die");


        QuestManager.Instance.ReportEvent(ObjectiveType.Kill, "lezard");

        Destroy(gameObject, 5f);
    }

    // Cette méthode sera appelée par une Animation Event
    public void DealDamage()
    {
        if (player == null)
            return;

        if (Vector3.Distance(transform.position, player.position) <= attackRange + 0.5f)
        {
            PlayerHealth health = player.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}