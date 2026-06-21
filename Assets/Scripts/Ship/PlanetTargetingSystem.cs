using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlanetTargetingSystem : MonoBehaviour
{
    public enum TargetMode
    {
        None,
        Planet,
        Asteroid
    }

    [Header("References")]
    public Transform playerShip;
    public Camera mainCamera;
    public RectTransform targetCursor;
    public Image targetCursorImage;
    public Text targetInfoText;
    public CanvasGroup targetInfoGroup;

    [Header("Planets")]
    public Transform[] planets;

    [Header("Settings")]
    public float maxTargetAngle = 60f;
    public float asteroidSearchRange = 3000f;
    public float focusRotationSpeed = 2f;
    public float cursorPulseSpeed = 5f;
    public float cursorPulseAmount = 12f;

    [Header("Colors")]
    public Color planetColor = Color.white;
    public Color asteroidColor = Color.red;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip lockSound;

    private TargetMode currentMode = TargetMode.None;
    private Transform selectedTarget;
    private float baseCursorSize = 140f;

    void Start()
    {
        if (targetCursorImage == null && targetCursor != null)
            targetCursorImage = targetCursor.GetComponent<Image>();

        HideTargetUI();
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.tKey.wasPressedThisFrame)
        {
            if (currentMode == TargetMode.Planet)
                ExitFocusMode();
            else
                EnterPlanetTargetMode();
        }

        if (keyboard.rKey.wasPressedThisFrame)
        {
            if (currentMode == TargetMode.Asteroid)
                ExitFocusMode();
            else
                EnterAsteroidTargetMode();
        }

        if (currentMode != TargetMode.None && selectedTarget != null)
        {
            if (keyboard.leftArrowKey.wasPressedThisFrame)
                SwitchTarget(-1);

            if (keyboard.rightArrowKey.wasPressedThisFrame)
                SwitchTarget(1);

            FocusCurrentTarget();
            UpdateCursor();
            UpdateTargetInfo();
        }

        if (currentMode != TargetMode.None && selectedTarget == null)
        {
            ExitFocusMode();
            return;
        }
    }

    public Transform GetSelectedTarget()
    {
        return selectedTarget;
    }

    void EnterPlanetTargetMode()
    {
        selectedTarget = GetBestPlanetInView();

        if (selectedTarget == null)
            return;

        currentMode = TargetMode.Planet;

        ShowTargetUI();

        if (targetCursorImage != null)
            targetCursorImage.color = planetColor;

        if (audioSource != null && lockSound != null)
            audioSource.PlayOneShot(lockSound);
    }

    void EnterAsteroidTargetMode()
    {
        selectedTarget = GetBestAsteroidInView();

        if (selectedTarget == null)
            return;

        currentMode = TargetMode.Asteroid;

        ShowTargetUI();

        if (targetCursorImage != null)
            targetCursorImage.color = asteroidColor;

        if (audioSource != null && lockSound != null)
            audioSource.PlayOneShot(lockSound);
    }

    void ExitFocusMode()
    {
        currentMode = TargetMode.None;
        selectedTarget = null;
        HideTargetUI();
    }

    void SwitchTarget(int direction)
    {
        Transform best = null;

        if (currentMode == TargetMode.Planet)
            best = GetNextPlanet(direction);

        if (currentMode == TargetMode.Asteroid)
            best = GetNextAsteroid(direction);

        if (best != null)
        {
            selectedTarget = best;

            if (audioSource != null && lockSound != null)
                audioSource.PlayOneShot(lockSound);
        }
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

            if (angle <= maxTargetAngle && angle < bestAngle)
            {
                bestAngle = angle;
                best = planet;
            }
        }

        return best;
    }

    Transform GetBestAsteroidInView()
    {
        AsteroidInteractable[] asteroids =
            FindObjectsByType<AsteroidInteractable>(FindObjectsSortMode.None);

        Transform best = null;
        float bestScore = float.MaxValue;

        foreach (AsteroidInteractable asteroid in asteroids)
        {
            if (asteroid == null) continue;

            Vector3 directionToAsteroid =
                asteroid.transform.position - playerShip.position;

            float distance = directionToAsteroid.magnitude;

            if (distance > asteroidSearchRange)
                continue;

            float angle = Vector3.Angle(playerShip.forward, directionToAsteroid);

            if (angle > maxTargetAngle)
                continue;

            Vector3 viewportPos =
                mainCamera.WorldToViewportPoint(asteroid.transform.position);

            if (viewportPos.z < 0)
                continue;

            float screenDistanceFromCenter =
                Vector2.Distance(
                    new Vector2(viewportPos.x, viewportPos.y),
                    new Vector2(0.5f, 0.5f)
                );

            float score =
                screenDistanceFromCenter * 1000f +
                distance * 0.01f;

            if (score < bestScore)
            {
                bestScore = score;
                best = asteroid.transform;
            }
        }

        return best;
    }

    Transform GetNextPlanet(int direction)
    {
        return GetNextFromList(planets, direction);
    }

    Transform GetNextAsteroid(int direction)
    {
        AsteroidInteractable[] asteroidComponents =
            FindObjectsByType<AsteroidInteractable>(FindObjectsSortMode.None);

        Transform[] asteroidTransforms = new Transform[asteroidComponents.Length];

        for (int i = 0; i < asteroidComponents.Length; i++)
            asteroidTransforms[i] = asteroidComponents[i].transform;

        return GetNextFromList(asteroidTransforms, direction);
    }

    Transform GetNextFromList(Transform[] targets, int direction)
    {
        if (selectedTarget == null || targets == null || targets.Length == 0)
            return null;

        Vector3 currentScreen = mainCamera.WorldToScreenPoint(selectedTarget.position);

        Transform bestTarget = null;
        float bestScore = float.MaxValue;

        foreach (Transform target in targets)
        {
            if (target == null || target == selectedTarget)
                continue;

            Vector3 screen = mainCamera.WorldToScreenPoint(target.position);

            if (screen.z < 0)
                continue;

            float distance = Vector3.Distance(playerShip.position, target.position);
            if (currentMode == TargetMode.Asteroid && distance > asteroidSearchRange)
                continue;

            float deltaX = screen.x - currentScreen.x;

            if (direction < 0 && deltaX >= 0)
                continue;

            if (direction > 0 && deltaX <= 0)
                continue;

            float deltaY = Mathf.Abs(screen.y - currentScreen.y);

            float score = Mathf.Abs(deltaX) + deltaY * 0.5f + distance * 0.001f;

            if (score < bestScore)
            {
                bestScore = score;
                bestTarget = target;
            }
        }

        return bestTarget;
    }

    void FocusCurrentTarget()
    {
        Vector3 direction = selectedTarget.position - playerShip.position;
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        playerShip.rotation = Quaternion.Slerp(
            playerShip.rotation,
            targetRotation,
            Time.deltaTime * focusRotationSpeed
        );
    }

    void UpdateCursor()
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(selectedTarget.position);

        if (viewportPos.z < 0)
            return;

        viewportPos.x = Mathf.Clamp(viewportPos.x, 0.08f, 0.92f);
        viewportPos.y = Mathf.Clamp(viewportPos.y, 0.08f, 0.92f);

        targetCursor.position = new Vector2(
            viewportPos.x * Screen.width,
            viewportPos.y * Screen.height
        );

        float pulse = Mathf.Sin(Time.time * cursorPulseSpeed) * cursorPulseAmount;
        float finalSize = baseCursorSize + pulse;

        targetCursor.sizeDelta = new Vector2(finalSize, finalSize);
    }

    void UpdateTargetInfo()
    {
        if (targetInfoText == null || selectedTarget == null)
            return;

        float distance = Vector3.Distance(playerShip.position, selectedTarget.position);

        string type = currentMode == TargetMode.Asteroid ? "ASTÉROÏDE" : "PLANÈTE";

        string name = selectedTarget.name;

        if (type == "ASTÉROÏDE")
        {
            name = "";
        }

        targetInfoText.text =
            type + "\n" +
            name +
            "\nDistance : " + Mathf.RoundToInt(distance) + " u";

        if (targetInfoGroup != null)
            targetInfoGroup.alpha = Mathf.Lerp(targetInfoGroup.alpha, 1f, Time.deltaTime * 5f);
    }

    void ShowTargetUI()
    {
        if (targetCursor != null)
            targetCursor.gameObject.SetActive(true);

        if (targetInfoText != null)
            targetInfoText.gameObject.SetActive(true);
    }

    void HideTargetUI()
    {
        if (targetCursor != null)
            targetCursor.gameObject.SetActive(false);

        if (targetInfoText != null)
            targetInfoText.gameObject.SetActive(false);

        if (targetInfoGroup != null)
            targetInfoGroup.alpha = 0f;
    }

    public void ClearTargetIf(Transform target)
    {
        if (selectedTarget == target)
        {
            ExitFocusMode();
        }
    }
}