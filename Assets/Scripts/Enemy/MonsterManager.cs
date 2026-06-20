using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class MonsterSpawnInfo
{
    public GameObject prefab;
    public int amount;
}

[System.Serializable]
public class SpawnZone
{
    public string zoneName = "Zone";

    // Optional: drag an empty GameObject here (e.g. "ZoneVolcan",
    // "ZoneRuines") placed where you want in the scene. If set, its
    // position overrides zoneCenter below — this lets you move the
    // zone around visually with the Move tool instead of typing
    // coordinates by hand.
    public Transform zoneAnchor;

    public Vector3 zoneCenter;
    public Vector3 zoneSize = new Vector3(20, 0, 20);
    public MonsterSpawnInfo[] monstersToSpawn;

    public Vector3 GetCenter()
    {
        return zoneAnchor != null ? zoneAnchor.position : zoneCenter;
    }
}

public class MonsterManager : MonoBehaviour
{
    // NOTE: no more singleton + DontDestroyOnLoad. Each scene keeps its
    // own MonsterManager with its own zones, so a manager persisting
    // across a scene change would just block the next scene's manager
    // from ever running (that was the "monsters disappear after
    // changing scene" bug). Instead, what persists is the monster
    // *data*, kept in a static dictionary keyed by scene name.
    public static MonsterManager Instance { get; private set; }

    [Header("Spawn Zones")]
    public SpawnZone[] zones;

    private class MonsterRecord
    {
        public GameObject prefab;
        public Vector3 position;
    }

    // sceneName -> (monsterId -> record). Static so it survives scene
    // unloads even though each MonsterManager instance itself doesn't.
    private static readonly Dictionary<string, Dictionary<string, MonsterRecord>> savedMonstersByScene =
        new Dictionary<string, Dictionary<string, MonsterRecord>>();

    // sceneName -> already spawned once this session.
    private static readonly HashSet<string> spawnedScenes = new HashSet<string>();

    private Dictionary<string, MonsterRecord> monsterRecords;
    private string sceneName;

    private void Awake()
    {
        Instance = this;

        sceneName = gameObject.scene.name;

        if (!savedMonstersByScene.TryGetValue(sceneName, out monsterRecords))
        {
            monsterRecords = new Dictionary<string, MonsterRecord>();
            savedMonstersByScene[sceneName] = monsterRecords;
        }
    }

    private void Start()
    {
        SpawnOrRestoreMonsters();
    }

    public void SaveMonster(Monster monster)
    {
        if (!monsterRecords.TryGetValue(monster.monsterId, out MonsterRecord record))
        {
            record = new MonsterRecord { prefab = monster.sourcePrefab };
            monsterRecords[monster.monsterId] = record;
        }

        record.position = monster.transform.position;
    }

    void SpawnOrRestoreMonsters()
    {
        if (spawnedScenes.Contains(sceneName))
            return;

        if (monsterRecords.Count > 0)
        {
            foreach (var entry in monsterRecords)
            {
                MonsterRecord record = entry.Value;

                if (record.prefab == null)
                {
                    Debug.LogWarning($"Monstre {entry.Key} : prefab d'origine inconnu, impossible de le restaurer.");
                    continue;
                }

                Vector3 pos = record.position;

                if (NavMesh.SamplePosition(pos, out NavMeshHit navHit, 5f, NavMesh.AllAreas))
                {
                    pos = navHit.position;
                }

                GameObject go = Instantiate(record.prefab, pos, Quaternion.identity);
                go.SetActive(true);

                Monster monster = go.GetComponent<Monster>();
                monster.monsterId = entry.Key;
                monster.sourcePrefab = record.prefab;
            }
        }
        else
        {
            foreach (SpawnZone zone in zones)
            {
                foreach (MonsterSpawnInfo monsterType in zone.monstersToSpawn)
                {
                    for (int i = 0; i < monsterType.amount; i++)
                    {
                        Vector3 pos = zone.GetCenter() + new Vector3(
                            Random.Range(-zone.zoneSize.x / 2f, zone.zoneSize.x / 2f),
                            0,
                            Random.Range(-zone.zoneSize.z / 2f, zone.zoneSize.z / 2f)
                        );

                        // Snap onto the baked NavMesh so the agent isn't
                        // spawned in a spot it's never "on".
                        if (NavMesh.SamplePosition(pos, out NavMeshHit navHit, 100f, NavMesh.AllAreas))
                        {
                            pos = navHit.position;
                        }
                        else
                        {
                            Debug.LogWarning($"Aucun NavMesh trouvé près de {pos} (zone \"{zone.zoneName}\"), le monstre risque de ne pas pouvoir se déplacer.");
                        }

                        GameObject go = Instantiate(monsterType.prefab, pos, Quaternion.identity);
                        go.SetActive(true); 

                        Monster monster = go.GetComponent<Monster>();
                        monster.monsterId = System.Guid.NewGuid().ToString();
                        monster.sourcePrefab = monsterType.prefab;

                        monsterRecords[monster.monsterId] = new MonsterRecord
                        {
                            prefab = monsterType.prefab,
                            position = pos
                        };
                    }
                }
            }
        }

        spawnedScenes.Add(sceneName);
    }

    private void OnDrawGizmos()
    {
        if (zones == null)
            return;

        Gizmos.color = Color.green;

        foreach (SpawnZone zone in zones)
        {
            Gizmos.DrawWireCube(zone.GetCenter(), zone.zoneSize);
        }
    }
}