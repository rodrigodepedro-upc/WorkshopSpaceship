using UnityEngine;
using System.Collections;

/// <summary>
/// Regenerates a shield after it is destroyed. When the shield's <see cref="Health"/>
/// reaches zero (and the shield object is disabled), this waits an optional delay,
/// refills the health progressively, and then re-enables the shield object once full.
///
/// Place this on an always-active object (e.g. the ship), NOT on the shield itself —
/// the shield gets disabled on death and could not run its own regen.
/// </summary>
public class ShieldRegen : MonoBehaviour
{
    [Tooltip("Health of the shield to regenerate.")]
    [SerializeField] Health shieldHealth = null;
    [Tooltip("Shield object to re-enable once fully regenerated. Defaults to the shield Health's object.")]
    [SerializeField] GameObject shieldObject = null;
    [Tooltip("Seconds to wait after destruction before regen begins.")]
    [SerializeField] float regenDelay = 2f;
    [Tooltip("Health restored per second while regenerating.")]
    [SerializeField] float regenPerSecond = 20f;

    Coroutine regenRoutine;

    void Awake()
    {
        if (shieldObject == null && shieldHealth != null)
            shieldObject = shieldHealth.gameObject;
    }

    void OnEnable()
    {
        if (shieldHealth != null)
            shieldHealth.OnDeath.AddListener(BeginRegen);
    }

    void OnDisable()
    {
        if (shieldHealth != null)
            shieldHealth.OnDeath.RemoveListener(BeginRegen);
    }

    void BeginRegen()
    {
        if (regenRoutine == null)
            regenRoutine = StartCoroutine(RegenRoutine());
    }

    IEnumerator RegenRoutine()
    {
        yield return new WaitForSeconds(regenDelay);

        // Refill while the shield object stays disabled.
        while (shieldHealth.HealthFraction < 1f)
        {
            shieldHealth.Heal(regenPerSecond * Time.deltaTime);
            yield return null;
        }

        shieldObject.SetActive(true);
        regenRoutine = null;
    }
}
