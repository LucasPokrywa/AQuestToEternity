using UnityEngine;
using UnityEngine.InputSystem;

public class LaserShooter : MonoBehaviour
{
    public Camera playerCamera;
    public LineRenderer laserLine;

    public float range = 10000f;
    public float fireRate = 0.12f;
    public float laserDuration = 0.06f;

    public float laserStartDistance = 0f;
    public float laserVerticalOffset = 8f;

    public int damage = 1;

    private float nextFireTime;

    void Start()
    {
        if (laserLine != null)
        {
            laserLine.enabled = false;
            laserLine.useWorldSpace = true;
            laserLine.positionCount = 2;

            laserLine.startWidth = 0.8f;
            laserLine.endWidth = 0.35f;
        }
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Fire();
        }
    }

    void Fire()
    {
        if (playerCamera == null || laserLine == null)
            return;

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Vector3 start =
            ray.GetPoint(laserStartDistance)
            + playerCamera.transform.up * laserVerticalOffset;

        Vector3 end = ray.GetPoint(range);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            end = hit.point;

            Destructible target =
                hit.collider.GetComponent<Destructible>();

            if (target != null)
                target.TakeDamage(damage);
        }

        laserLine.SetPosition(0, start);
        laserLine.SetPosition(1, end);

        laserLine.enabled = true;

        CancelInvoke(nameof(HideLaser));
        Invoke(nameof(HideLaser), laserDuration);
    }

    void HideLaser()
    {
        if (laserLine != null)
            laserLine.enabled = false;
    }
}