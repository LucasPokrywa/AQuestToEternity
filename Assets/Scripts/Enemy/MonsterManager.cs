using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class MonsterSpawnInfo
{
    public GameObject prefab;
    public int amount;
}

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    [Header("Spawn")]
    public MonsterSpawnInfo[] monstersToSpawn;
    public Vector3 zoneCenter;
    public Vector3 zoneSize = new Vector3(20, 0, 20);

    private Dictionary<string, Vector3> monsterPositions =
        new Dictionary<string, Vector3>();

    private bool monstersSpawned = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnOrRestoreMonsters();
    }

    public void SaveMonster(Monster monster)
    {
        monsterPositions[monster.monsterId] = monster.transform.position;
    }

    void SpawnOrRestoreMonsters()
    {
        if (monstersSpawned)
            return;

        if (monsterPositions.Count > 0)
        {
            foreach (var monster in monsterPositions)
            {
                Vector3 pos = monster.Value;

                if (NavMesh.SamplePosition(pos, out NavMeshHit navHit, 5f, NavMesh.AllAreas))
                {
                    pos = navHit.position;
                }

                GameObject go = Instantiate(
                    monstersToSpawn[0].prefab,
                    pos,
                    Quaternion.identity
                );

                go.SetActive(true); 

                go.GetComponent<Monster>().monsterId = monster.Key;
            }
        }
        else
        {
            foreach (MonsterSpawnInfo monsterType in monstersToSpawn)
            {
                for (int i = 0; i < monsterType.amount; i++)
                {
                    Vector3 pos = zoneCenter + new Vector3(
                        Random.Range(-zoneSize.x / 2f, zoneSize.x / 2f),
                        0,
                        Random.Range(-zoneSize.z / 2f, zoneSize.z / 2f)
                    );

                    if (NavMesh.SamplePosition(pos, out NavMeshHit navHit, 5f, NavMesh.AllAreas))
                    {
                        pos = navHit.position;
                    }
                    else
                    {
                        Debug.LogWarning($"Aucun NavMesh trouvé près de {pos}, le monstre risque de ne pas pouvoir se déplacer.");
                    }

                    GameObject go = Instantiate(
                        monsterType.prefab,
                        pos,
                        Quaternion.identity
                    );

                    go.SetActive(true); 

                    Monster monster = go.GetComponent<Monster>();

                    monster.monsterId = System.Guid.NewGuid().ToString();

                    monsterPositions[monster.monsterId] = pos;
                }
            }
        }

        monstersSpawned = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(zoneCenter, zoneSize);
    }
}