using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum QuestStatus
{
    NotStarted,
    Active,
    Completed,
    Failed
}

public class Quest
{
    public QuestData data;

    public QuestStatus status;

    public List<Objective> objectives = new();


    public bool IsCompleted() => objectives.All(o => o.IsCompleted());

    public void UpdateObjective(string targetID, int amount = 1)
    {
        foreach (var obj in objectives)
            if (obj.data.targetID == targetID && !obj.IsCompleted())
                obj.currentAmount += amount;
    }
}

public class Objective
{
    public ObjectiveData data;
    public int currentAmount;
    public bool IsCompleted() => currentAmount >= data.requiredAmount;
}