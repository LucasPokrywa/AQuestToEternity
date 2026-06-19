using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public Text checklistText;

    public int asteroidsCollected = 0;
    public int asteroidsDestroyed = 0;

    public bool landedOnMercure = false;
    public bool landedOnVenus = false;

    void Start()
    {
        UpdateChecklist();
    }

    public void CollectAsteroid()
    {
        asteroidsCollected++;
        UpdateChecklist();
    }

    public void DestroyAsteroid()
    {
        asteroidsDestroyed++;
        UpdateChecklist();
    }

    public void LandOnMercury()
    {
        landedOnMercure = true;
        UpdateChecklist();
    }

    public void LandOnVenus()
    {
        landedOnVenus = true;
        UpdateChecklist();
    }

    void UpdateChecklist()
    {
        checklistText.text =
            ChecklistLine(asteroidsCollected >= 3, "Collecter 3 astéroïdes " + asteroidsCollected + "/3") + "\n" +
            ChecklistLine(landedOnMercure, "Atterrir sur Mercure") + "\n" +
            ChecklistLine(landedOnVenus, "Atterrir sur Vénus") + "\n" +
            ChecklistLine(asteroidsDestroyed >= 5, "Détruire 5 astéroïdes " + asteroidsDestroyed + "/5");
    }

    string ChecklistLine(bool done, string text)
    {
        return (done ? "☑ " : "☐ ") + text;
    }
}