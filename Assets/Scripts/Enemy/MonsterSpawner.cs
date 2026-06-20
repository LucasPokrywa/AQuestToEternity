using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int monsterCount = 10;

    public Vector3 zoneSize = new Vector3(20, 0, 20);

    void Start()
    {
        SpawnMonsters();
    }

    void SpawnMonsters()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            Vector3 randomPos = transform.position + new Vector3(
                Random.Range(-zoneSize.x / 2, zoneSize.x / 2),
                0,
                Random.Range(-zoneSize.z / 2, zoneSize.z / 2)
            );

            Instantiate(monsterPrefab, randomPos, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}