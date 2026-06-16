using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlanetTargetingSystem : MonoBehaviour
{
    [Header("References")]
    public Transform playerShip;
    public Camera mainCamera;
    public RectTransform targetCursor;
    public Text targetInfoText;
    public CanvasGroup targetInfoGroup;
    public AudioSource audioSource;
    public AudioClip lockSound;

    [Header("Planets")]
    public Transform[] planets;

    [Header("Settings")]
    public float maxTargetAngle = 60f;
    public float focusRotationSpeed = 2f;
    public float cursorPulseSpeed = 5f;
    public float cursorPulseAmount = 12f;

    private bool focusMode = false;
    private Transform selectedPlanet;
    private float baseCursorSize = 140f;

    void Start()
    {
        if (targetCursor != null)
            targetCursor.gameObject.SetActive(false);

        if (targetInfoText != null)
            targetInfoText.gameObject.SetActive(false);

        if (targetInfoGroup != null)
            targetInfoGroup.alpha = 0f;
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Toggle focus
        if (keyboard.tKey.wasPressedThisFrame)
        {
            if (focusMode)
                ExitFocusMode();
            else
                EnterFocusMode();
        }

        if (!focusMode || selectedPlanet == null)
            return;

        // Switch gauche / droite
        if (keyboard.leftArrowKey.wasPressedThisFrame)
            SwitchTarget(-1);

        if (keyboard.rightArrowKey.wasPressedThisFrame)
            SwitchTarget(1);

        FocusCurrentTarget();
        UpdateCursor();
        UpdateTargetInfo();
    }

    void EnterFocusMode()
    {
        selectedPlanet = GetBestPlanetInView();

        if (selectedPlanet == null)
            return;

        focusMode = true;

        if (targetCursor != null)
            targetCursor.gameObject.SetActive(true);

        if (targetInfoText != null)
            targetInfoText.gameObject.SetActive(true);

        if (audioSource != null && lockSound != null)
            audioSource.PlayOneShot(lockSound);
    }

    void ExitFocusMode()
    {
        focusMode = false;
        selectedPlanet = null;

        if (targetCursor != null)
            targetCursor.gameObject.SetActive(false);

        if (targetInfoText != null)
            targetInfoText.gameObject.SetActive(false);

        if (targetInfoGroup != null)
            targetInfoGroup.alpha = 0f;
    }

    Transform GetBestPlanetInView()
    {
        Transform best = null;
        float bestAngle = float.MaxValue;

        foreach (Transform planet in planets)
        {
            if (planet == null) continue;

            Vector3 dir = planet.position - playerShip.position;
            float angle = Vector3.Angle(playerShip.forward, dir);

            if (angle > maxTargetAngle)
                continue;

            if (angle < bestAngle)
            {
                bestAngle = angle;
                best = planet;
            }
        }

        return best;
    }

    void SwitchTarget(int direction)
    {
        if (selectedPlanet == null) return;

        Vector3 currentScreen = mainCamera.WorldToScreenPoint(selectedPlanet.position);

        Transform bestPlanet = null;
        float bestScore = float.MaxValue;

        foreach (Transform planet in planets)
        {
            if (planet == null || planet == selectedPlanet)
                continue;

            Vector3 screen = mainCamera.WorldToScreenPoint(planet.position);

            if (screen.z < 0)
                continue;

            float deltaX = screen.x - currentScreen.x;

            if (direction < 0 && deltaX >= 0)
                continue;

            if (direction > 0 && deltaX <= 0)
                continue;

            float deltaY = Mathf.Abs(screen.y - currentScreen.y);
            float score = Mathf.Abs(deltaX) + deltaY * 0.5f;

            if (score < bestScore)
            {
                bestScore = score;
                bestPlanet = planet;
            }
        }

        if (bestPlanet != null)
        {
            selectedPlanet = bestPlanet;

            if (audioSource != null && lockSound != null)
                audioSource.PlayOneShot(lockSound);
        }
    }

    void FocusCurrentTarget()
    {
        Vector3 direction = selectedPlanet.position - playerShip.position;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        playerShip.rotation = Quaternion.Slerp(
            playerShip.rotation,
            targetRotation,
            Time.deltaTime * focusRotationSpeed
        );
    }

    void UpdateCursor()
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(selectedPlanet.position);

        bool behind = viewportPos.z < 0;

        if (behind)
        {
            viewportPos.x = 1f - viewportPos.x;
            viewportPos.y = 1f - viewportPos.y;
        }

        viewportPos.x = Mathf.Clamp(viewportPos.x, 0.08f, 0.92f);
        viewportPos.y = Mathf.Clamp(viewportPos.y, 0.08f, 0.92f);

        Vector2 screenPos = new Vector2(
            viewportPos.x * Screen.width,
            viewportPos.y * Screen.height
        );

        float distance = Vector3.Distance(playerShip.position, selectedPlanet.position);

        float planetSize = Mathf.Max(
            selectedPlanet.localScale.x,
            selectedPlanet.localScale.y,
            selectedPlanet.localScale.z
        );

        baseCursorSize = 140f;

        if (!behind && distance < planetSize * 25f)
        {
            Vector3 center = mainCamera.WorldToScreenPoint(selectedPlanet.position);

            Vector3 edge = mainCamera.WorldToScreenPoint(
                selectedPlanet.position + mainCamera.transform.up * planetSize * 0.5f
            );

            float radius = Vector2.Distance(
                new Vector2(center.x, center.y),
                new Vector2(edge.x, edge.y)
            );

            baseCursorSize = Mathf.Clamp(radius * 2.2f, 90f, 180f);
        }

        float pulse = Mathf.Sin(Time.time * cursorPulseSpeed) * cursorPulseAmount;
        float finalSize = baseCursorSize + pulse;

        targetCursor.gameObject.SetActive(true);
        targetCursor.position = screenPos;
        targetCursor.sizeDelta = new Vector2(finalSize, finalSize);
    }

    void UpdateTargetInfo()
    {
        if (targetInfoText == null || selectedPlanet == null)
            return;

        float distance = Vector3.Distance(playerShip.position, selectedPlanet.position);

        string display = Mathf.RoundToInt(distance) + " u";

        targetInfoText.text =
            selectedPlanet.name +
            "\nDistance : " + display;

        if (targetInfoGroup != null)
            targetInfoGroup.alpha = Mathf.Lerp(targetInfoGroup.alpha, 1f, Time.deltaTime * 5f);
    }
}