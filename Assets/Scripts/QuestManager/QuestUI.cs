using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance { get; private set; }

    private Text checklistText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        FindChecklistText();
    }

    private void FindChecklistText()
    {
        GameObject checklistGO = GameObject.Find("ChecklistMission");

        if (checklistGO == null)
        {
            Debug.Log("❌ ChecklistMission introuvable dans la scène !");
            return;
        }

        checklistText = checklistGO.GetComponent<Text>();

        if (checklistText == null)
        {
            Debug.Log("❌ Aucun Text trouvé sur ChecklistMission !");
        }
    }

    private void Update()
    {
        if (checklistText == null)
        {
            FindChecklistText();
            return;
        }

        UpdateChecklist();
    }

    void UpdateChecklist()
    {
        if (QuestManager.Instance == null) return;

        var sb = new StringBuilder();

        foreach (var quest in QuestManager.Instance.ActiveQuests)
        {
            sb.AppendLine($"<size=60>{quest.data.questName}</size>");
            sb.AppendLine($"<size=22>{quest.data.description}</size>");
            sb.AppendLine("─────────────────────────");

            foreach (var obj in quest.objectives)
            {
                sb.AppendLine(
                    (obj.IsCompleted() ? "☑ " : "☐ ") +
                    $"{obj.data.description} {obj.currentAmount}/{obj.data.requiredAmount}"
                );
            }

            sb.AppendLine("");
        }

        checklistText.text = sb.ToString();
    }
}