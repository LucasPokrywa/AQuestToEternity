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
    public Text bottomRightText;

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

        if (topWarningText != null)
        {
            topWarningText.gameObject.SetActive(dangerLevel > 0.05f);
            topWarningText.text = "⚠ SIGNAL PERDU — DÉSYNCHRONISATION ⚠";
            topWarningText.color = new Color(1f, 0f, 0f, Mathf.Lerp(0.4f, 1f, dangerLevel));
        }
    }

    public void TriggerGameOver()
    {
        isGameOver = true;

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

        if (bottomRightText != null)
        {
            bottomRightText.gameObject.SetActive(true);
            bottomRightText.text = "STATUT: CRITIQUE\nCONNEXION: PERDUE\nRETOUR AU MENU...";
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
    }

    private void HideGameOverUI()
    {
        if (topWarningText != null)
            topWarningText.gameObject.SetActive(false);

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        if (subtitleText != null)
            subtitleText.gameObject.SetActive(false);

        if (bottomRightText != null)
            bottomRightText.gameObject.SetActive(false);
    }

    private void ShakeUI(float amount)
    {
        if (canvasRoot == null)
            return;

        canvasRoot.anchoredPosition = originalCanvasPos + Random.insideUnitCircle * amount;
    }
}