using UnityEngine;
using UnityEngine.InputSystem;

public class LaserShooterShip : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject laserProjectilePrefab;
    public PlanetTargetingSystem targetingSystem;
    public MissionManager missionManager;

    public float fireRate = 0.25f;
    public float projectileStartDistance = 20f;
    public float projectileVerticalOffset = -2f;

    private float nextFireTime;

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Fire();
        }
    }

    void Fire()
    {
        if (playerCamera == null || laserProjectilePrefab == null)
            return;

        Vector3 start =
            playerCamera.transform.position +
            playerCamera.transform.forward * projectileStartDistance +
            playerCamera.transform.up * projectileVerticalOffset;

        Transform target = null;

        if (targetingSystem != null)
            target = targetingSystem.GetSelectedTarget();

        Vector3 direction;

        if (target != null)
            direction = target.position - start;
        else
            direction = playerCamera.transform.forward;

        GameObject projectile = Instantiate(
            laserProjectilePrefab,
            start,
            Quaternion.LookRotation(direction)
        );

        LaserProjectile laser = projectile.GetComponent<LaserProjectile>();

        if (laser != null)
            laser.Init(direction, target, targetingSystem, missionManager);
    }
}