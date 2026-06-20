using System.Collections.Generic;
using UnityEngine;

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
    public List<QuestData> prerequisites;
}

[System.Serializable]
public class ObjectiveData
{
    public string description;
    public ObjectiveType type;
    public string targetID;
    public int requiredAmount;
}