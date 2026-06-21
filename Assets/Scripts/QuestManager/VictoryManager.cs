using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    [Header("Cinématique & Caméra")]
    public Transform victoryCameraPivot;
    public Camera victoryCamera;
    public GameObject player;
    public float cameraOrbitSpeed = 15f;

    [Header("UI Cinématique")]
    public CanvasGroup victoryCanvasGroup;
    public RectTransform blackBarTop;
    public RectTransform blackBarBottom;
    public float blackBarHeight = 150f;

    [Header("UI à cacher")]
    [Tooltip("Glissez ici tous les Canvas du jeu (vie, quêtes, radar...) pour les faire disparaître")]
    public GameObject[] otherUIToHide;

    [Header("Audio")]
    [Tooltip("La musique épique de victoire")]
    public AudioSource victoryMusic;

    // --- NOUVEAU : Couper la musique de fond ---
    [Tooltip("La musique de fond du niveau à couper lors de la victoire")]
    public AudioSource backgroundMusic;

    private bool isVictoryTriggered = false;

    void Start()
    {
        if (victoryCamera != null) victoryCamera.gameObject.SetActive(false);
        if (victoryCanvasGroup != null) victoryCanvasGroup.alpha = 0f;

        if (blackBarTop != null) blackBarTop.sizeDelta = new Vector2(blackBarTop.sizeDelta.x, 0);
        if (blackBarBottom != null) blackBarBottom.sizeDelta = new Vector2(blackBarBottom.sizeDelta.x, 0);
    }

    void Update()
    {
        if (isVictoryTriggered && victoryCameraPivot != null)
        {
            victoryCameraPivot.Rotate(Vector3.up * cameraOrbitSpeed * Time.unscaledDeltaTime);
        }
    }

    public void TriggerVictory()
    {
        if (isVictoryTriggered) return;
        isVictoryTriggered = true;

        StartCoroutine(VictorySequence());
    }

    private IEnumerator VictorySequence()
    {
        // 1. Désactiver le joueur
        if (player != null) player.SetActive(false);

        // 2. Cacher tout le reste de l'interface
        foreach (GameObject ui in otherUIToHide)
        {
            if (ui != null) ui.SetActive(false);
        }

        // 3. Allumer la caméra cinématique
        if (victoryCamera != null) victoryCamera.gameObject.SetActive(true);

        // --- NOUVEAU : Gestion propre de l'Audio ---
        // On coupe la musique de fond avant de lancer celle de victoire
        if (backgroundMusic != null) backgroundMusic.Stop();
        if (victoryMusic != null) victoryMusic.Play();

        // 4. Effet de Slow Motion dramatique
        Time.timeScale = 0.3f;

        // 5. Animation des bandes noires cinématiques
        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;

            if (blackBarTop != null)
                blackBarTop.sizeDelta = new Vector2(blackBarTop.sizeDelta.x, Mathf.Lerp(0, blackBarHeight, t));

            if (blackBarBottom != null)
                blackBarBottom.sizeDelta = new Vector2(blackBarBottom.sizeDelta.x, Mathf.Lerp(0, blackBarHeight, t));

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        // 6. Fade In doux du texte de victoire
        t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 0.5f;
            if (victoryCanvasGroup != null) victoryCanvasGroup.alpha = t;
            yield return null;
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}