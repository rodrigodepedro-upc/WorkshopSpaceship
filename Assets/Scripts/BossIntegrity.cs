using UnityEngine;
using System.Collections;

/// <summary>
/// The boss's overall "structural integrity", implemented as a <see cref="Health"/>
/// so a HealthBar can track it directly. Each part's <see cref="BossPart"/> mirrors
/// the damage it takes onto this via <see cref="TakeDamage"/>.
///
/// The integrity's max is a fraction of the parts' combined health (so the boss
/// dies before every part is destroyed — e.g. 0.5 means it explodes once half the
/// parts' total health has been dealt as damage). Because it dies at zero, its
/// health bar empties exactly when the boss explodes. On death EVERY
/// ExplosionController under the boss is triggered, not just parts with a Health.
/// </summary>
public class BossIntegrity : Health
{
    [Header("Boss")]
    [Tooltip("Parts whose combined max health the integrity is derived from. Leave empty to use this component's own Max Health.")]
    [SerializeField] Health[] parts = new Health[0];
    [Tooltip("Integrity max as a fraction of the parts' combined health (0.5 = boss explodes after half their total health is dealt).")]
    [SerializeField, Range(0f, 1f)] float integrityFraction = 0.5f;
    [Tooltip("Seconds after the boss explodes before the boss object is destroyed. Removing it stops systems that outlive it (e.g. shield regen re-enabling the shield). Should cover the longest explosion clip.")]
    [SerializeField] float destroyDelay = 2f;

    bool exploded = false;

    protected override void Awake()
    {
        // Integrity max = a fraction of the parts' combined health, so the bar
        // fills to that amount and empties exactly when the boss explodes.
        float partsTotal = 0f;
        foreach (Health part in parts)
            if (part != null)
                partsTotal += part.MaxHealth;

        if (partsTotal > 0f)
            maxHealth = partsTotal * integrityFraction;

        base.Awake();   // sets currentHealth = maxHealth

        OnDeath.AddListener(Explode);
    }

    /// <summary>Mirror a part's damage onto the boss integrity. Called from BossPart.</summary>
    public void TakeDamage(float amount)
    {
        if (exploded)
            return;

        DealDamage(amount);
    }

    void Explode()
    {
        if (exploded)
            return;

        exploded = true;

        // Trigger every explosion in the boss, including parts with no Health.
        // Already-destroyed parts are gone from the hierarchy, so they're skipped.
        Ship8.ExplosionController[] explosions = GetComponentsInChildren<Ship8.ExplosionController>();

        foreach (Ship8.ExplosionController explosion in explosions)
            explosion.StartExplosion();

        // Remove the boss once its explosions have played. This also tears down
        // components living on the boss (e.g. ShieldRegen), so the shield can't
        // regenerate and reappear after the boss is gone.
        StartCoroutine(DestroyAfterExplosion());
    }

    IEnumerator DestroyAfterExplosion()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
