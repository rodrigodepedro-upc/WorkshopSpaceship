using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer textureA = null;
    [SerializeField] private SpriteRenderer textureB = null;
    [SerializeField] private float speed = 1f;

    void Update()
    {
        // Scroll both textures down by the same amount this frame.
        Vector3 step = Vector3.down * (speed * Time.deltaTime);
        textureA.transform.position += step;
        textureB.transform.position += step;

        SpriteRenderer upper = textureA.transform.position.y > textureB.transform.position.y ? textureA : textureB;
        SpriteRenderer lower = (upper == textureA) ? textureB : textureA;

        // Once the upper texture has scrolled past the origin, stack the lower one directly above it.
        if (upper.transform.position.y < 0f)
            lower.transform.position = upper.transform.position + Vector3.up * upper.size.y;
    }
}