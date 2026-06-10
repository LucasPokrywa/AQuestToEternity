using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlanetLandingDetector : MonoBehaviour
{
    public string planetName = "Earth";
    public GameObject landingText;

    private bool playerIsNear = false;

    void Start()
    {
        landingText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;

            landingText.SetActive(true);

            landingText.GetComponent<Text>().text =
                "Appuie sur E pour atterrir sur " + planetName;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;

            landingText.SetActive(false);
        }
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (playerIsNear && keyboard.eKey.wasPressedThisFrame)
        {
            landingText.GetComponent<Text>().text =
                "Atterrissage sur " + planetName + " !";
        }
    }
}