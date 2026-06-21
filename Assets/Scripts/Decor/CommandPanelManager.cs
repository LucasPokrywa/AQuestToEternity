using UnityEngine;
using UnityEngine.UI;

public class CommandPanelManager : MonoBehaviour
{
    [Header("Texts")]
    public Text actionsText;
    public Text deplacementText;

    [Header("Buttons")]
    public Image systemeSolaireButtonImage;
    public Image planetesButtonImage;

    [Header("Colors")]
    public Color selectedColor = new Color(0.2f, 0.4f, 1f);
    public Color normalColor = Color.white;

    private void OnEnable()
    {
        ShowSystemeSolaireCommands();
    }

    public void ShowSystemeSolaireCommands()
    {
        deplacementText.text =
            "Z : Avancer\n" +
            "Q/D : Tourner\n" +
            "Espace : Monter\n" +
            "Ctrl : Descendre";

        actionsText.text =
            "T : Cibler planète\n" +
            "R : Cibler astéroïde\n" +
            "← / → : Changer de cible\n" +
            "Clic gauche : Tirer\n" +
            "E : Atterrir / Collecter\n" +
            "F : Quitter siège";

        UpdateButtonColors(true);
    }

    public void ShowPlanetesCommands()
    {
        deplacementText.text =
            "Z/Q/S/D : Se déplacer\n" +
            "Souris : Regarder\n" +
            "Espace : Sauter";

        actionsText.text =
            "E : Interagir\n" +
            "Clic gauche : Tirer";

        UpdateButtonColors(false);
    }

    private void UpdateButtonColors(bool systemeSolaireSelected)
    {
        if (systemeSolaireButtonImage != null)
            systemeSolaireButtonImage.color =
                systemeSolaireSelected ? selectedColor : normalColor;

        if (planetesButtonImage != null)
            planetesButtonImage.color =
                systemeSolaireSelected ? normalColor : selectedColor;
    }
}