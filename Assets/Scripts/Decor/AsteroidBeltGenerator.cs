using UnityEngine;

public class AsteroidBeltGenerator : MonoBehaviour
{
    public GameObject asteroidPrefab;

    public int asteroidCount = 200;

    public float innerRadius = 1900f;
    public float outerRadius = 2400f;

    public float heightVariation = 80f;

    public float minScale = 5f;
    public float maxScale = 25f;

    void Start()
    {
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

            GameObject asteroid = Instantiate(
                asteroidPrefab,
                transform.position + position,
                Random.rotation,
                transform
            );

            float scale = Random.Range(minScale, maxScale);
            asteroid.transform.localScale = Vector3.one * scale;
        }
    }
}