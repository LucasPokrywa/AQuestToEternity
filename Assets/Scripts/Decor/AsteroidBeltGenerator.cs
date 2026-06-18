using UnityEngine;

public class AsteroidBeltGenerator : MonoBehaviour
{
    public Transform asteroids; // Groupe contenant Cube.000 à Cube.019

    private GameObject[] asteroidModels;

    public int asteroidCount = 200;

    public float innerRadius = 1900f;
    public float outerRadius = 2400f;

    public float heightVariation = 80f;

    public float minRotationSpeed = 0.5f; // degrés/seconde
    public float maxRotationSpeed = 3f;

    public float minOrbitSpeed = 0.02f; // degrés/seconde, très lent
    public float maxOrbitSpeed = 0.15f;

    void Start()
    {
        // Récupère tous les modèles enfants
        int count = asteroids.childCount;
        asteroidModels = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            asteroidModels[i] = asteroids.GetChild(i).gameObject;
        }

        GenerateBelt();
    }

    void GenerateBelt()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(innerRadius, outerRadius);

            Vector3 position = new Vector3(
                Mathf.Cos(angle) * radius,
                Random.Range(-heightVariation, heightVariation),
                Mathf.Sin(angle) * radius
            );


            // Choix aléatoire du modèle
            GameObject prefab = asteroidModels[Random.Range(0, asteroidModels.Length)];

            GameObject asteroid = Instantiate(
                prefab,
                transform.position + position,
                Random.rotation,
                transform
            );

            // Scale entre 0.7 et 1.5
            float scale = Random.Range(1.5f, 4.5f);
            asteroid.transform.localScale = Vector3.one * scale;

            // Rotation sur lui-même, axe et sens aléatoires, vitesse très légère
            AsteroidSelfRotation rotator = asteroid.AddComponent<AsteroidSelfRotation>();
            Vector3 randomAxis = Random.onUnitSphere;
            float randomSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
            float randomOrbitSpeed = Random.Range(minOrbitSpeed, maxOrbitSpeed);
            rotator.Init(randomAxis, randomSpeed, transform.position, randomOrbitSpeed);
        }
    }
}