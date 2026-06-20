using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 80f;
    public float rotationSpeed = 70f;
    public float verticalSpeed = 50f;

    [Header("Planet Blocking Triggers")]
    public Collider[] blockingTriggers;
    public float shipRadius = 1f;

    [Header("Engine Trails")]
    public TrailRenderer leftTrail;
    public TrailRenderer rightTrail;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetTrails(false);
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        float moveForward = 0f;
        float turn = 0f;
        float moveUp = 0f;

        if (keyboard.zKey.isPressed || keyboard.wKey.isPressed)
            moveForward = 1f;

        if (keyboard.sKey.isPressed)
            moveForward = -1f;

        if (keyboard.qKey.isPressed || keyboard.aKey.isPressed)
            turn = -1f;

        if (keyboard.dKey.isPressed)
            turn = 1f;

        if (keyboard.spaceKey.isPressed)
            moveUp = 1f;

        if (keyboard.leftCtrlKey.isPressed)
            moveUp = -1f;

        SetTrails(moveForward > 0f);

        transform.Rotate(Vector3.up * turn * rotationSpeed * Time.deltaTime);

        Vector3 movement =
            transform.forward * moveForward * moveSpeed * Time.deltaTime +
            transform.up * moveUp * verticalSpeed * Time.deltaTime;

        Vector3 nextPosition = transform.position + movement;

        if (!WouldEnterBlockingTrigger(nextPosition))
        {
            transform.position = nextPosition;
        }
    }

    private void SetTrails(bool active)
    {
        if (leftTrail != null)
            leftTrail.emitting = active;

        if (rightTrail != null)
            rightTrail.emitting = active;
    }

    private bool WouldEnterBlockingTrigger(Vector3 nextPosition)
    {
        foreach (Collider trigger in blockingTriggers)
        {
            if (trigger == null)
                continue;

            Vector3 closestPoint = trigger.ClosestPoint(nextPosition);
            float distance = Vector3.Distance(nextPosition, closestPoint);

            if (distance < shipRadius)
                return true;
        }

        return false;
    }
}