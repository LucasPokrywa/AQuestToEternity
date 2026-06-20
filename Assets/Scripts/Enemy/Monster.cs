using UnityEngine;

public class Monster : MonoBehaviour
{
    [System.NonSerialized]
    public string monsterId;

    private void Awake()
    {
        if (string.IsNullOrEmpty(monsterId))
            monsterId = System.Guid.NewGuid().ToString();
    }

    private void Update()
    {
        if (MonsterManager.Instance != null)
        {
            MonsterManager.Instance.SaveMonster(this);
            
        }
    }
}