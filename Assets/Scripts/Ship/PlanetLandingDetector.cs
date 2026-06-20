using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlanetLandingDetector : MonoBehaviour
{
    [SerializeField] private string planetName;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneToLoad;
#endif

    [SerializeField] private string sceneName;
    [SerializeField] private GameObject landingText;
    [SerializeField] private GameObject landingPanel;
    [SerializeField] private MissionManager missionManager;

    private bool playerIsNear = false;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (sceneToLoad != null)
            sceneName = sceneToLoad.name;
#endif
    }

    private void Awake()
    {
        if (landingText != null)
            landingText.SetActive(false);

        if (landingPanel != null)
            landingPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerIsNear = true;

        if (landingText != null)
        {
            landingText.SetActive(true);

            Text textComponent = landingText.GetComponent<Text>();
            if (textComponent != null)
                textComponent.text = "Appuie sur E pour atterrir sur " + planetName;
        }

        if (landingPanel != null)
            landingPanel.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerIsNear = false;

        if (landingText != null)
            landingText.SetActive(false);

        if (landingPanel != null)
            landingPanel.SetActive(false);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (playerIsNear && keyboard.eKey.wasPressedThisFrame)
        {
            if (missionManager != null)
            {
                if (planetName == "Mercure")
                    missionManager.LandOnMercury();

                if (planetName == "Venus")
                    missionManager.LandOnVenus();
            }

            if (!string.IsNullOrEmpty(sceneName))
                SceneManager.LoadScene(sceneName);
            else
                Debug.LogError("Aucune scène assignée pour " + planetName);
        }
    }
}