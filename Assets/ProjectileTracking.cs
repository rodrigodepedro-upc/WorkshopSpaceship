using UnityEngine;

/// <summary>
/// Gently steers a Rigidbody2D-driven projectile (e.g. a launched bomb) toward a
/// target Transform. It rotates the existing velocity a small amount each physics
/// step while preserving its speed, so the projectile curves slightly rather than
/// homing sharply. Keep <see cref="turnSpeed"/> low for a subtle nudge.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileTracking : MonoBehaviour
{
    [Tooltip("Maximum degrees per second the trajectory bends toward the target. Small = a slight curve.")]
    [SerializeField] float turnSpeed = 20f;
    [Tooltip("Rotate the sprite to face its travel direction (sprite up = velocity).")]
    [SerializeField] bool faceTravelDirection = true;

    Rigidbody2D rb;
    Transform target;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        Vector2 velocity = rb.linearVelocity;
        float speed = velocity.magnitude;

        if (speed < Mathf.Epsilon)
            return;

        // Direction we'd ideally travel, then bend the current velocity toward it
        // by at most turnSpeed degrees this step, keeping the original speed.
        Vector2 desired = ((Vector2)target.position - rb.position).normalized * speed;
        float maxRadians = turnSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime;
        Vector2 steered = Vector3.RotateTowards(velocity, desired, maxRadians, 0f);

        rb.linearVelocity = steered.normalized * speed;

        if (faceTravelDirection)
            transform.up = rb.linearVelocity;
    }
}
