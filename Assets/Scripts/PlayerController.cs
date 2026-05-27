using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] List<ProjectileSpawner> playerProjectileSpawners = null;
    [SerializeField] SpriteRenderer shipSprite = null;
    [SerializeField] float speed = 1f;

    BoxCollider2D shipCollider = null;

    private void Start()
    {
        shipCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement

        Vector3 inputDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.A)) inputDirection.x -= 1f;
        if (Input.GetKey(KeyCode.D)) inputDirection.x += 1f;

        if (Input.GetKey(KeyCode.S)) inputDirection.y -= 1f;
        if (Input.GetKey(KeyCode.W)) inputDirection.y += 1f;

        Vector3 movement = inputDirection.normalized * speed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;

        // Position Clamping

        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;            // world units from center to top
        float halfWidth = halfHeight * cam.aspect;          // ... to the right edge

        Vector2 padding = shipCollider.size / 2f;

        newPosition.x = Mathf.Clamp(newPosition.x, -halfWidth + padding.x, halfWidth - padding.x);
        newPosition.y = Mathf.Clamp(newPosition.y, -halfHeight + padding.y, 0f);

        transform.position = newPosition;

        // Shooting

        if (Input.GetKey(KeyCode.Space))
            foreach(ProjectileSpawner spawner in playerProjectileSpawners) spawner.Shoot();
    }
}
