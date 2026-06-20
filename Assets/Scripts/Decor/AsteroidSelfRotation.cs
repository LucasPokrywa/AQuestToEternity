using UnityEngine;

public class AsteroidSelfRotation : MonoBehaviour
{
    private Vector3 rotationAxis;
    private float rotationSpeed;

    private Vector3 orbitCenter;
    private float orbitSpeed; // degrés/seconde

    public void Init(Vector3 axis, float speed, Vector3 center, float orbitSpd)
    {
        rotationAxis = axis;
        rotationSpeed = speed;
        orbitCenter = center;
        orbitSpeed = orbitSpd;
    }

    void Update()
    {
        // Rotation sur lui-même
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);

        // Déplacement lent autour du centre de la ceinture
        transform.RotateAround(orbitCenter, Vector3.up, orbitSpeed * Time.deltaTime);
    }
}