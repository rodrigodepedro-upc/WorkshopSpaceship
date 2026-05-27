using UnityEngine;
using System.Collections;

public class MovementCoroutine : MonoBehaviour
{
    [SerializeField] Vector2 movement;
    [SerializeField] float time = 1f;
    [SerializeField] float delay = 0f;

    public void Execute()
    {
        StartCoroutine(Move(movement));
    }

    /// <summary>Plays the original movement backwards, returning to the start position.</summary>
    public void Revert()
    {
        enabled = true;   // Execute disables the component when it finishes.
        StartCoroutine(Move(-movement));
    }

    IEnumerator Move(Vector2 delta)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = originalPosition + (Vector3)delta;

        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;

        enabled = false;
    }
}
