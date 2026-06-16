using UnityEngine;

public class ChangerModele : MonoBehaviour
{

    public GameObject baliseModel;

    public GameObject baseModel;

    // Faire en sorte qui se le joueur est dans les parage alors proposer UI et machiner
    // Verifier condition inventaire    
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            Changer();
        }*/
    }

    public void Changer()
    {
        baliseModel.SetActive(false);

        baseModel.SetActive(true);
    }
}