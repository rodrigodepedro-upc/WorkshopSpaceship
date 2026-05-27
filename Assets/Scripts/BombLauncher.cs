using UnityEngine;

/// <summary>
/// Animation-driven bomb launcher for the Ship8 bomb carrier.
///
/// The firing cadence does NOT spawn the bomb directly. Instead it sets the
/// Animator's "BombLaunch" trigger, which plays the arms-open clip. That clip
/// carries an animation event calling <see cref="LaunchBomb"/>, which spawns the
/// bomb in sync with the arms releasing it.
///
/// Call <see cref="RequestLaunch"/> manually (input/UnityEvent) or enable
/// <see cref="autoFire"/> to request a launch every <see cref="fireCooldown"/> seconds.
/// </summary>
public class BombLauncher : MonoBehaviour
{
    const string LaunchTrigger = "BombLaunch";

    [SerializeField] GameObject bombPrefab = null;
    [Tooltip("Where the bomb spawns. It launches straight down in world space.")]
    [SerializeField] Transform bombStartPos = null;
    [SerializeField] float bombSpeed = 5f;
    [Tooltip("Optional target the launched bomb will steer toward, if it has a ProjectileTracking.")]
    [SerializeField] Transform target = null;

    [Header("Firing")]
    [Tooltip("When enabled, requests a launch continuously every fireCooldown seconds.")]
    [SerializeField] bool autoFire = false;
    [Tooltip("Seconds between launch requests.")]
    [SerializeField] float fireCooldown = 1f;

    Animator animator;
    float cooldownTimer = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (autoFire)
            RequestLaunch();
    }

    /// <summary>
    /// Plays the launch animation if the cooldown has elapsed. Safe to call every
    /// frame — it self-rate-limits. The bomb itself is spawned by the animation
    /// event (<see cref="LaunchBomb"/>), keeping the arms and bomb in sync.
    /// </summary>
    public void RequestLaunch()
    {
        if (cooldownTimer > 0f || animator == null)
            return;

        cooldownTimer = fireCooldown;
        animator.SetTrigger(LaunchTrigger);
    }

    public void SetAutoFire(bool enabled)
    {
        autoFire = enabled;
    }

    /// <summary>
    /// Spawns and launches the bomb. Invoked by the "LaunchBomb" animation event
    /// in the arms-open clip — do not call directly, or it bypasses the animation.
    /// </summary>
    public void LaunchBomb()
    {
        if (bombPrefab == null || bombStartPos == null)
            return;

        GameObject bomb = Instantiate(bombPrefab, bombStartPos.position, bombStartPos.rotation);

        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = bombSpeed * Vector2.down;

        ProjectileTracking tracking = bomb.GetComponent<ProjectileTracking>();
        if (tracking != null)
            tracking.SetTarget(target);
    }
}
