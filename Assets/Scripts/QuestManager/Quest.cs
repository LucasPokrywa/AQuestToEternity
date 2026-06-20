using System.Collections.Generic;
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

    public List<Objective> UpdateObjective(ObjectiveType type, string targetID, int amount = 1)
    {
        var updated = new List<Objective>();

        foreach (var obj in objectives)
        {
            if (obj.IsCompleted())
                continue;

            if (obj.data.type == type &&
                obj.data.targetID == targetID)
            {
                obj.currentAmount += amount;
                updated.Add(obj);
            }
        }

        return updated;
    }
}