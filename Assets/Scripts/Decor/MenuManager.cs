using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Jouer()
    {
        Debug.Log("Joiuer");
        SceneManager.LoadScene("Systeme_Solaire");
    }

    public void Quitter()
    {
        Application.Quit();
    }
}