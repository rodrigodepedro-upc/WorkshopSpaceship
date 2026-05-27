using UnityEngine;

/// <summary>
/// Bridges one destructible boss part to the boss's <see cref="BossIntegrity"/>.
/// Whenever the part takes damage, that amount is mirrored onto the integrity.
/// When the part's health reaches zero, its explosion plays (which removes the
/// part at the end of its clip).
///
/// The part's <see cref="Health"/> should have "disable on death" turned OFF, so
/// the object stays active long enough to play its explosion.
/// </summary>
[RequireComponent(typeof(Health))]
public class BossPart : MonoBehaviour
{
    [SerializeField] Health health = null;
    [SerializeField] BossIntegrity integrity = null;
    [Tooltip("Explosion that plays when this part is destroyed. Auto-found on this object if left empty.")]
    [SerializeField] Ship8.ExplosionController explosion = null;

    void Awake()
    {
        if (health == null)
            health = GetComponent<Health>();
        if (explosion == null)
            explosion = GetComponent<Ship8.ExplosionController>();
    }

    void OnEnable()
    {
        health.OnDamageTaken.AddListener(MirrorToIntegrity);
        health.OnDeath.AddListener(Explode);
    }

    void OnDisable()
    {
        health.OnDamageTaken.RemoveListener(MirrorToIntegrity);
        health.OnDeath.RemoveListener(Explode);
    }

    void MirrorToIntegrity(float amount)
    {
        if (integrity != null)
            integrity.TakeDamage(amount);
    }

    void Explode()
    {
        if (explosion != null)
            explosion.StartExplosion();
    }
}
