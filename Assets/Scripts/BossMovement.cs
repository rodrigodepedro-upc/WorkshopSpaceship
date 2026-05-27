using UnityEngine;

/// <summary>
/// Drives the boss across the screen in one of three patterns. The active
/// pattern is held in <see cref="currentMode"/>; the method that swaps between
/// modes is added later.
///
/// - Lateral: slides horizontally, bouncing off the left and right edges.
/// - Diagonal: lateral movement plus a vertical bounce, tracing a sawtooth path.
/// - Follow: matches the player's horizontal position while easing back to its
///   original vertical row.
/// </summary>
public class BossMovement : MonoBehaviour
{
    public enum MovementMode { Lateral, Diagonal, Follow }

    [SerializeField] SpriteRenderer bossSprite = null;
    [SerializeField] Transform player = null;
    [SerializeField] float speed = 1f;
    [SerializeField] MovementMode currentMode = MovementMode.Lateral;

    float startY = 0f;
    int horizontalDirection = 1;    // +1 moving right, -1 moving left
    int verticalDirection = 1;      // +1 moving up, -1 moving down (diagonal mode)

    private void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        switch (currentMode)
        {
            case MovementMode.Lateral: MoveLateral(); break;
            case MovementMode.Diagonal: MoveDiagonal(); break;
            case MovementMode.Follow: MoveFollow(); break;
        }
    }

    void MoveLateral()
    {
        Vector3 position = transform.position;
        Vector2 limit = ScreenLimit();

        position.x += horizontalDirection * speed * Time.deltaTime;

        if (position.x >= limit.x) { position.x = limit.x; horizontalDirection = -1; }
        else if (position.x <= -limit.x) { position.x = -limit.x; horizontalDirection = 1; }

        transform.position = position;
    }

    void MoveDiagonal()
    {
        Vector3 position = transform.position;
        Vector2 limit = ScreenLimit();

        position.x += horizontalDirection * speed * Time.deltaTime;
        position.y += verticalDirection * speed * Time.deltaTime;

        // Bounce on every edge so the combined motion zig-zags into a sawtooth.
        if (position.x >= limit.x) { position.x = limit.x; horizontalDirection = -1; }
        else if (position.x <= -limit.x) { position.x = -limit.x; horizontalDirection = 1; }

        if (position.y >= limit.y) { position.y = limit.y; verticalDirection = -1; }
        else if (position.y <= -limit.y) { position.y = -limit.y; verticalDirection = 1; }

        transform.position = position;
    }

    void MoveFollow()
    {
        if (player == null)
            return;

        Vector3 position = transform.position;
        Vector2 limit = ScreenLimit();

        float targetX = Mathf.Clamp(player.position.x, -limit.x, limit.x);
        position.x = Mathf.MoveTowards(position.x, targetX, speed * Time.deltaTime);
        position.y = Mathf.MoveTowards(position.y, startY, speed * Time.deltaTime);

        transform.position = position;
    }

    // Furthest the boss center can travel on each axis, padded by its sprite size.
    Vector2 ScreenLimit()
    {
        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;            // world units from center to top
        float halfWidth = halfHeight * cam.aspect;          // ... to the right edge

        Vector2 padding = bossSprite != null ? (Vector2)bossSprite.bounds.extents : Vector2.zero;

        return new Vector2(halfWidth - padding.x, halfHeight - padding.y);
    }
}
