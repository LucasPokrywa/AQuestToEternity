using UnityEngine;

public class WorldBoundary : MonoBehaviour
{
    public enum ZoneType { Warning, Death }
    public enum TriggerMode { EnterTrigger, ExitTrigger }

    public ZoneType zoneType;
    public TriggerMode triggerMode;

    public DesyncEffectManager effectManager;

    [Range(0f, 1f)]
    public float warningIntensity = 0.4f;

    private bool warningActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (triggerMode == TriggerMode.EnterTrigger)
            TriggerEffect();

        if (triggerMode == TriggerMode.ExitTrigger && zoneType == ZoneType.Warning)
            ClearWarning();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (triggerMode == TriggerMode.ExitTrigger)
            TriggerEffect();

        if (triggerMode == TriggerMode.EnterTrigger && zoneType == ZoneType.Warning)
            ClearWarning();
    }

    void TriggerEffect()
    {
        if (effectManager == null)
        {
            Debug.LogError("EffectManager manquant sur " + gameObject.name);
            return;
        }

        if (zoneType == ZoneType.Warning)
        {
            warningActive = true;
            effectManager.SetDangerLevel(warningIntensity);
        }

        if (zoneType == ZoneType.Death)
        {
            effectManager.TriggerGameOver();
        }
    }

    void ClearWarning()
    {
        if (!warningActive)
            return;

        warningActive = false;

        if (effectManager != null)
            effectManager.ClearDanger();
    }
}