using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private List<Quest> activeQuests = new();
    private List<QuestData> completedQuests = new();

    public event Action<Quest> OnQuestStarted;
    public event Action<Quest> OnQuestCompleted;
    public event Action<Quest, Objective> OnObjectiveUpdated;

    void Awake() => Instance = this;

    public bool TryStartQuest(QuestData data)
    {
        // Vérifie les prérequis
        foreach (var prereq in data.prerequisites)
            if (!completedQuests.Contains(prereq)) return false;

        var quest = new Quest { data = data, status = QuestStatus.Active };
        quest.objectives = data.objectives.Select(o => new Objective { data = o }).ToList();
        activeQuests.Add(quest);
        OnQuestStarted?.Invoke(quest);
        return true;
    }

    public void ReportEvent(string targetID, int amount = 1)
    {
        foreach (var quest in activeQuests)
        {
            quest.UpdateObjective(targetID, amount);
            if (quest.IsCompleted()) CompleteQuest(quest);
        }
    }

    private void CompleteQuest(Quest quest)
    {
        quest.status = QuestStatus.Completed;
        activeQuests.Remove(quest);
        completedQuests.Add(quest.data);
        OnQuestCompleted?.Invoke(quest);
        // Donner les récompenses ici
    }
}