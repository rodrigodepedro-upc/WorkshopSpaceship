using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;

    [SerializeField, ReadOnly] float currentHealth = 0f;

    [Tooltip("If off, reaching zero health fires OnDeath but does NOT disable the object (let an explosion handle removal). Use for boss parts.")]
    [SerializeField] bool disableOnDeath = true;

    [Header("Invulnerability")]
    [Tooltip("If enabled, taking damage starts an invulnerability window during which the sprite flickers and further hits are ignored. Use for the player.")]
    [SerializeField] bool hasInvulnerability = false;
    [Tooltip("Sprite toggled on/off during the invulnerability flicker. Auto-found on this object if left empty.")]
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [Tooltip("How long the invulnerability window lasts after a hit.")]
    [SerializeField] float invulnerabilityDuration = 1f;
    [Tooltip("Seconds between each on/off flicker toggle during the window.")]
    [SerializeField] float flickerInterval = 0.1f;

    // Fires whenever health changes, passing the normalized value (0..1).
    public event Action<float> OnHealthChanged;

    [Header("Events")]
    [Tooltip("Fires when a hit lands (after invulnerability is checked). Wire flickers/SFX here, e.g. the enemy's HitFlash.Flash.")]
    public UnityEvent OnHit = new UnityEvent();
    [Tooltip("Fires when health reaches zero, just before the object is disabled.")]
    public UnityEvent OnDeath = new UnityEvent();
    [Tooltip("Fires when damage is taken, passing the amount. Used to mirror part damage onto the boss integrity.")]
    public UnityEvent<float> OnDamageTaken = new UnityEvent<float>();

    public float HealthFraction => currentHealth / maxHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0f;

    bool isInvulnerable = false;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Let listeners (e.g. the health bar) initialize to full.
        OnHealthChanged?.Invoke(HealthFraction);
    }

    public void DealDamage(float damage)
    {
        // Ignore hits once dead/dying, so an exploding part doesn't re-trigger
        // OnHit/OnDeath (which would flash an already-torn-down sprite).
        if (isInvulnerable || damage <= 0f || IsDead)
            return;

        currentHealth -= damage;

        OnHit.Invoke();
        OnDamageTaken.Invoke(damage);

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
        else if (hasInvulnerability)
        {
            StartCoroutine(InvulnerabilityRoutine());
        }

        OnHealthChanged?.Invoke(HealthFraction);
    }

    /// <summary>Restores health (e.g. shield regen). Works while the object is disabled.</summary>
    public void Heal(float amount)
    {
        if (amount <= 0f)
            return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(HealthFraction);
    }

    IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float elapsed = 0f;
        while (elapsed < invulnerabilityDuration)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flickerInterval);
            elapsed += flickerInterval;
        }

        // Always end visible and vulnerable again.
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        isInvulnerable = false;
    }

    void Die()
    {
        OnDeath.Invoke();

        if (disableOnDeath)
            gameObject.SetActive(false);
    }
}
