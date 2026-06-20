using UnityEngine;
using UnityEngine.InputSystem;

public class LaserShooterShip : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject laserProjectilePrefab;
    public PlanetTargetingSystem targetingSystem;
    public MissionManager missionManager;

    [Header("Spawn")]
    public Transform laserSpawnPoint;

    [Header("Aim")]
    public float aimScreenYOffset = 60f;

    [Header("Fire")]
    public float fireRate = 0.25f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip laserSound;

    private float nextFireTime;

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Fire();
        }
    }

    void Fire()
    {
        if (playerCamera == null || laserProjectilePrefab == null)
            return;

        if (audioSource != null && laserSound != null)
        {
            audioSource.PlayOneShot(laserSound);
        }

        Vector3 start;

        if (laserSpawnPoint != null)
        {
            start = laserSpawnPoint.position;
        }
        else
        {
            start =
                playerCamera.transform.position +
                playerCamera.transform.forward * 20f;
        }

        Transform target = null;

        if (targetingSystem != null)
        {
            target = targetingSystem.GetSelectedTarget();
        }

        Vector3 direction;

        if (target != null)
        {
            direction = target.position - start;
        }
        else
        {
            Vector3 screenPoint = new Vector3(
                Screen.width * 0.5f,
                Screen.height * 0.5f + aimScreenYOffset,
                0f
            );

            Ray ray = playerCamera.ScreenPointToRay(screenPoint);

            Vector3 aimPoint = ray.GetPoint(10000f);

            direction = aimPoint - start;
        }

        GameObject projectile = Instantiate(
            laserProjectilePrefab,
            start,
            Quaternion.LookRotation(direction)
        );

        LaserProjectile laser =
            projectile.GetComponent<LaserProjectile>();

        if (laser != null)
        {
            laser.Init(
                direction,
                target,
                targetingSystem,
                missionManager
            );
        }
    }
}