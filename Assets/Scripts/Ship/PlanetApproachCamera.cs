using UnityEngine;

public class PlanetApproachCamera : MonoBehaviour
{
    public Transform playerShip;
    public Camera mainCamera;

    public Transform[] planets;

    public float farFOV = 80f;
    public float nearFOV = 70f;
    public float smoothSpeed = 2f;

    void Update()
    {
        if (playerShip == null || mainCamera == null || planets == null || planets.Length == 0)
            return;

        Transform bestPlanet = null;
        float bestScore = float.MinValue;

        foreach (Transform planet in planets)
        {
            if (planet == null) continue;

            float distance = Vector3.Distance(playerShip.position, planet.position);
            float size = planet.localScale.x;

            float score = size / Mathf.Max(distance, 1f);

            if (score > bestScore)
            {
                bestScore = score;
                bestPlanet = planet;
            }
        }

        if (bestPlanet != null)
        {
            float distance = Vector3.Distance(playerShip.position, bestPlanet.position);
            float size = bestPlanet.localScale.x;

            float minDistance = size * 3f;
            float maxDistance = size * 20f;

            float t = Mathf.InverseLerp(maxDistance, minDistance, distance);
            float targetFOV = Mathf.Lerp(farFOV, nearFOV, t);

            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * smoothSpeed);
        }
    }
}