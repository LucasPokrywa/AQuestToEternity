using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float speed = 800f;
    public float lifeTime = 3f;

    private Vector3 direction;
    private Transform target;
    private PlanetTargetingSystem targetingSystem;
    private MissionManager missionManager;

    public void Init(Vector3 dir, Transform newTarget, PlanetTargetingSystem system, MissionManager manager)
    {
        direction = dir.normalized;
        target = newTarget;
        targetingSystem = system;
        missionManager = manager;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        AsteroidInteractable asteroid = other.GetComponentInParent<AsteroidInteractable>();

        if (asteroid != null)
        {
            Transform destroyedTarget = asteroid.transform;

            if (targetingSystem != null)
                targetingSystem.ClearTargetIf(destroyedTarget);

            QuestManager.Instance.ReportEvent(ObjectiveType.Kill, "asteroides");

            Destroy(asteroid.gameObject);
            Destroy(gameObject);
        }
    }
}