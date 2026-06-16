using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 80f;
    public float rotationSpeed = 70f;
    public float verticalSpeed = 50f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        float moveForward = 0f;
        float turn = 0f;
        float moveUp = 0f;

        // Avancer / Reculer
        if (keyboard.zKey.isPressed || keyboard.wKey.isPressed)
            moveForward = 1f;

        if (keyboard.sKey.isPressed)
            moveForward = -1f;

        // Rotation gauche / droite
        if (keyboard.qKey.isPressed || keyboard.aKey.isPressed)
            turn = -1f;

        if (keyboard.dKey.isPressed)
            turn = 1f;

        // Monter / Descendre
        if (keyboard.spaceKey.isPressed)
            moveUp = 1f;

        if (keyboard.leftCtrlKey.isPressed)
            moveUp = -1f;

        // Déplacement
        transform.Translate(
            Vector3.forward * moveForward * moveSpeed * Time.deltaTime
        );

        // Rotation
        transform.Rotate(
            Vector3.up * turn * rotationSpeed * Time.deltaTime
        );

        // Vertical
        transform.Translate(
            Vector3.up * moveUp * verticalSpeed * Time.deltaTime
        );
    }
}