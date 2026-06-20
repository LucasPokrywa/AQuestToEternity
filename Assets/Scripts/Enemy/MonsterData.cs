using UnityEngine;

[System.Serializable]
public class MonsterData
{
    public string id;
    public Vector3 position;

    public MonsterData(string id, Vector3 position)
    {
        this.id = id;
        this.position = position;
    }
}