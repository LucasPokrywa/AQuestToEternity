using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public enum ObjectiveType
{
    Kill,
    Collect,
    TalkTo,
    ReachLocation
}

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest")]
public class QuestData : ScriptableObject
{
    public string questName;
    public string description;
    public List<ObjectiveData> objectives;
    public List<QuestData> prerequisites; // quêtes requises avant
    public int rewardXP;
    public Item rewardItem; // optionnel
}

[System.Serializable]
public class ObjectiveData
{
    public string description;       // "Tuer 5 loups"
    public ObjectiveType type;       // Kill, Collect, TalkTo, ReachLocation
    public string targetID;          // "Wolf", "HealthPotion", etc.
    public int requiredAmount;
}