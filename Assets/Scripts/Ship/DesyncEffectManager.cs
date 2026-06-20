using UnityEngine;
using UnityEngine.UI;

public class DesyncEffectManager : MonoBehaviour
{
    [Header("Noise")]
    public DynamicSignalNoise noise;

    [Header("Texts")]
    public Text topWarningText;
    public Text gameOverText;
    public Text subtitleText;

    [Header("Mission UI")]
    public GameObject missionChecklistUI;

    [Header("Canvas")]
    public RectTransform canvasRoot;

    [Header("Player")]
    public GameObject playerObject;

    [Range(0f, 1f)]
    public float dangerLevel = 0f;

    private bool isGameOver = false;
    private Vector2 originalCanvasPos;

    void Start()
    {
        dangerLevel = 0f;

        if (canvasRoot != null)
            originalCanvasPos = canvasRoot.anchoredPosition;

        if (noise != null)
            noise.intensity = 0f;

        HideGameOverUI();
        SetDangerLevel(0f);
    }

    void Update()
    {
        if (isGameOver)
        {
            SetDangerLevel(1f);
            ShakeUI(4f);
            return;
        }

        ShakeUI(dangerLevel * 3f);
    }

    public void SetDangerLevel(float value)
    {
        dangerLevel = Mathf.Clamp01(value);

        if (noise != null)
            noise.intensity = dangerLevel;

        bool isDesync = dangerLevel > 0.05f;

        if (missionChecklistUI != null)
            missionChecklistUI.SetActive(!isDesync);

        if (topWarningText != null)
        {
            topWarningText.gameObject.SetActive(isDesync);
            topWarningText.text = "⚠ SIGNAL PERDU — DÉSYNCHRONISATION ⚠";
            topWarningText.color = new Color(1f, 0f, 0f, Mathf.Lerp(0.4f, 1f, dangerLevel));
        }
    }

    public void TriggerGameOver()
    {
        isGameOver = true;

        if (missionChecklistUI != null)
            missionChecklistUI.SetActive(false);

        if (noise != null)
            noise.intensity = 1f;

        if (topWarningText != null)
        {
            topWarningText.gameObject.SetActive(true);
            topWarningText.text = "⚠ SIGNAL PERDU — DÉSYNCHRONISATION ⚠";
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER";
        }

        if (subtitleText != null)
        {
            subtitleText.gameObject.SetActive(true);
            subtitleText.text = "VOUS AVEZ ÉTÉ DÉCONNECTÉ DU RÉSEAU";
        }

        if (playerObject != null)
        {
            ShipController ship = playerObject.GetComponent<ShipController>();
            if (ship != null)
                ship.enabled = false;
        }
    }

    public void ClearDanger()
    {
        if (isGameOver)
            return;

        SetDangerLevel(0f);

        if (missionChecklistUI != null)
            missionChecklistUI.SetActive(true);
    }

    private void HideGameOverUI()
    {
        if (topWarningText != null)
            topWarningText.gameObject.SetActive(false);

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        if (subtitleText != null)
            subtitleText.gameObject.SetActive(false);
    }

    private void ShakeUI(float amount)
    {
        if (canvasRoot == null)
            return;

        canvasRoot.anchoredPosition =
            originalCanvasPos + Random.insideUnitCircle * amount;
    }
}