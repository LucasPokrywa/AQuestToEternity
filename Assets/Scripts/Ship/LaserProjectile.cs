using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float speed = 800f;
    public float lifeTime = 3f;

    private Vector3 direction;
    private Transform target;
    private PlanetTargetingSystem targetingSystem;

    public void Init(Vector3 dir, Transform newTarget, PlanetTargetingSystem system)
    {
        direction = dir.normalized;
        target = newTarget;
        targetingSystem = system;

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

            Destroy(asteroid.gameObject);
            Destroy(gameObject);
        }
    }
}