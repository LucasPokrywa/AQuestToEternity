using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenu;
    public GameObject commandPanel;
    public GameObject synopsis;

    private void Start()
    {
        mainMenu.SetActive(true);
        commandPanel.SetActive(false);
    }

    public void Jouer()
    {
        SceneManager.LoadScene("Systeme_Solaire");
    }

    public void OuvrirCommandes()
    {
        mainMenu.SetActive(false);
        commandPanel.SetActive(true);
    }

    public void RetourMenu()
    {
        commandPanel.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void Synopsis()
    {
        commandPanel.SetActive(false);
        mainMenu.SetActive(false);
        synopsis.SetActive(true);
    }

    public void Quitter()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}