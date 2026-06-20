using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InteractChangeScene : MonoBehaviour
{
    [Header("Player / Camera")]
    public Transform playerCamera;
    public float activationDistance = 3f;

    [Header("UI")]
    public TextMeshProUGUI promptText;

    [Header("Scene (sélection dans Inspector)")]
#if UNITY_EDITOR
    public SceneAsset sceneToLoadAsset;
#endif
    private string sceneToLoad;

    [Header("Mission Requise")]
    public QuestData missionRequiseVaisseau;


    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main.transform;

#if UNITY_EDITOR
        if (sceneToLoadAsset != null)
            sceneToLoad = sceneToLoadAsset.name;
#endif

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (QuestManager.Instance.IsQuestCompleted(missionRequiseVaisseau))
        {
            float distance = Vector3.Distance(playerCamera.position, transform.position);
            bool isNear = distance <= activationDistance;
            if (promptText != null)
            {
                promptText.gameObject.SetActive(isNear);
                promptText.text = "E pour embarquer";
            }

            if (isNear && Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneToLoadAsset != null)
            sceneToLoad = sceneToLoadAsset.name;
    }
#endif
}