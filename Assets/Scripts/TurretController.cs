using UnityEngine;

/// <summary>
/// Aims a turret at a target Transform (snapping rotation to a configurable
/// angle step) and fires projectiles through one or more
/// <see cref="ProjectileSpawner"/> muzzles.
///
/// Set <see cref="autoFire"/> to have the turret fire continuously while it has
/// a target; the cadence is governed by each muzzle's own cooldown. Other scripts
/// can also trigger a shot manually through <see cref="Fire"/>.
/// </summary>
public class TurretController : MonoBehaviour
{
    [Header("Aiming")]
    [Tooltip("Transform that rotates to face the target. Defaults to this object's transform.")]
    [SerializeField] Transform turret = null;
    [Tooltip("Transform the turret tracks and shoots at. Can be assigned at runtime.")]
    [SerializeField] Transform target = null;
    [Tooltip("Snap rotation to multiples of this angle (degrees). Use 1 for smooth aim, higher for stepped pixel-art aim.")]
    [SerializeField] float deltaAngle = 1f;

    [Header("Firing")]
    [Tooltip("Muzzles that spawn the projectiles. Each fires along its own up axis, so parent them to the turret pointing down the barrel. Their cooldown governs the fire rate.")]
    [SerializeField] ProjectileSpawner[] muzzles = new ProjectileSpawner[0];
    [Tooltip("Fire one muzzle per shot in sequence instead of all muzzles at once.")]
    [SerializeField] bool fireInSequence = false;
    [Tooltip("In sequence mode, seconds between consecutive muzzle shots (the stagger between muzzles).")]
    [SerializeField] float sequenceInterval = 0.1f;
    [Tooltip("When enabled, the turret fires continuously while it has a target.")]
    [SerializeField] bool autoFire = false;

    int muzzleIndex = 0;
    float sequenceTimer = 0f;

    void Awake()
    {
        if (turret == null)
            turret = transform;
    }

    void Update()
    {
        AimAtTarget();

        if (sequenceTimer > 0f)
            sequenceTimer -= Time.deltaTime;

        if (autoFire)
            Fire();
    }

    void AimAtTarget()
    {
        if (turret == null || target == null)
            return;

        // Direction from the target back to the turret, flattened to 2D.
        Vector3 direction = turret.position - target.position;
        direction.z = 0f;

        // Angle of that direction off the up axis, resolved to a full 0-360 turn.
        float angle = Vector3.Angle(Vector3.up, direction);
        if (target.position.x - turret.position.x < 0f)
            angle = 360f - angle;

        // Snap to the nearest multiple of deltaAngle for stepped pixel-art aim.
        if (deltaAngle > 0f)
            angle = Mathf.Round(angle / deltaAngle) * deltaAngle;

        turret.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    /// <summary>
    /// Triggers the muzzles. Safe to call every frame — each muzzle's
    /// <see cref="ProjectileSpawner"/> self-rate-limits with its own cooldown.
    /// </summary>
    public void Fire()
    {
        if (muzzles.Length == 0)
            return;

        if (fireInSequence)
        {
            // Stagger consecutive muzzles, otherwise they'd fire on back-to-back
            // frames (the next muzzle's spawner cooldown isn't running yet).
            if (sequenceTimer > 0f)
                return;

            FireMuzzle(muzzleIndex);
            muzzleIndex = (muzzleIndex + 1) % muzzles.Length;
            sequenceTimer = sequenceInterval;
        }
        else
        {
            for (int i = 0; i < muzzles.Length; i++)
                FireMuzzle(i);
        }
    }

    void FireMuzzle(int index)
    {
        if (muzzles[index] != null)
            muzzles[index].Shoot();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetAutoFire(bool enabled)
    {
        autoFire = enabled;
    }
}
