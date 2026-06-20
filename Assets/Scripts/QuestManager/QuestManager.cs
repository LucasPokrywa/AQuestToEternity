using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Objective
{
    public ObjectiveData data;
    public int currentAmount;
    public bool IsCompleted() => currentAmount >= data.requiredAmount;
}
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private List<Quest> activeQuests = new();
    private List<QuestData> completedQuests = new();

    [Header("Toutes les quêtes du jeu")]
    public List<QuestData> allQuests;

    public event Action<Quest> OnQuestStarted;
    public event Action<Quest> OnQuestCompleted;
    public event Action<Quest, Objective> OnObjectiveUpdated;

    public IReadOnlyList<Quest> ActiveQuests => activeQuests;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null) return;

        var prefab = Resources.Load<QuestManager>("QuestManager");
        if (prefab == null)
        {
            Debug.LogError("❌ QuestManager prefab introuvable dans Resources/");
            return;
        }

        var instance = Instantiate(prefab);
        instance.name = "QuestManager";
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log($"ℹ️ [Quest] Initialisation. {allQuests?.Count ?? 0} quête(s).");
        TryStartAvailableQuests();
    }

    public bool TryStartQuest(QuestData data)
    {
        foreach (var prereq in data.prerequisites)
        {
            if (!completedQuests.Contains(prereq))
            {
                Debug.Log($"⏳ '{data.questName}' bloquée (prérequis manquant)");
                return false;
            }
        }

        var quest = new Quest
        {
            data = data,
            status = QuestStatus.Active,
            objectives = data.objectives.Select(o => new Objective { data = o }).ToList()
        };

        activeQuests.Add(quest);

        Debug.Log($"📋 NOUVELLE QUÊTE : {data.questName}");

        OnQuestStarted?.Invoke(quest);
        return true;
    }

    private void TryStartAvailableQuests()
    {
        foreach (var data in allQuests)
        {
            if (completedQuests.Contains(data)) continue;
            if (activeQuests.Any(q => q.data == data)) continue;

            TryStartQuest(data);
        }
    }
    public bool IsQuestCompleted(QuestData data)
    {
        return completedQuests.Contains(data);
    }
    public void ReportEvent(ObjectiveType type, string targetID, int amount = 1)
    {
        foreach (var quest in activeQuests.ToList())
        {
            var updated = quest.UpdateObjective(type, targetID, amount);

            foreach (var obj in updated)
            {
                OnObjectiveUpdated?.Invoke(quest, obj);
            }

            if (quest.IsCompleted())
            {
                CompleteQuest(quest);
            }
        }

        DebugQuestState(); 
    }

    public void ReportEvent(ObjectiveType type, GameObject target, int amount = 1)
    {
        if (target == null) return;

        string id = target.name.Replace("(Clone)", "").Trim();
        ReportEvent(type, id, amount);

        DebugQuestState(); 
    }

    private void DebugQuestState()
    {
        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        Debug.Log($"📊 ACTIVE QUESTS: {activeQuests.Count}");

        foreach (var quest in activeQuests)
        {
            Debug.Log($"🧭 Quest: {quest.data.questName} [{quest.status}]");

            foreach (var obj in quest.objectives)
            {
                Debug.Log(
                    $"   ↳ {obj.data.description} " +
                    $"({obj.data.type} | {obj.data.targetID}) " +
                    $"=> {obj.currentAmount}/{obj.data.requiredAmount}" +
                    $"{(obj.IsCompleted() ? " ✅" : "")}"
                );
            }

            Debug.Log($"   ✔ Completed: {quest.IsCompleted()}");
        }

        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
    }
    private void CompleteQuest(Quest quest)
    {
        quest.status = QuestStatus.Completed;


        Debug.Log("Quete terminé");

        activeQuests.Remove(quest);
        completedQuests.Add(quest.data);

        OnQuestCompleted?.Invoke(quest);

        TryStartAvailableQuests();
    }

    public bool AreAllQuestsCompleted()
    {
        return allQuests.All(q => completedQuests.Contains(q));
    }
}