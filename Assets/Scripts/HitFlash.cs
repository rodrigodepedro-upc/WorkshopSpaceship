using UnityEngine;
using System.Collections;

/// <summary>
/// Briefly flashes the sprite white. Call <see cref="Flash"/> to trigger it —
/// wire it into a <see cref="Health.OnHit"/> event slot in the inspector so it
/// plays on every hit, without any code subscription.
///
/// The white-out is done by swapping to a "flash" material for the duration.
/// Assign a material whose shader outputs solid white (keeping the sprite's
/// alpha) — see the notes shared with this script.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class HitFlash : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [Tooltip("Material that renders the sprite solid white.")]
    [SerializeField] Material flashMaterial = null;
    [Tooltip("Seconds each white blink lasts.")]
    [SerializeField] float flashDuration = 0.08f;
    [Tooltip("How many times to blink per hit.")]
    [SerializeField] int flashCount = 1;

    Material originalMaterial;
    Coroutine flashRoutine;

    void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalMaterial = spriteRenderer.material;
    }

    /// <summary>Plays the flash. Restarts cleanly if already flashing.</summary>
    public void Flash()
    {
        // Bail if there's nothing to flash or the object is on its way out
        // (e.g. mid-explosion) — StartCoroutine would throw on an inactive object.
        if (flashMaterial == null || spriteRenderer == null || !isActiveAndEnabled)
            return;

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            if (spriteRenderer == null)
                yield break;
            spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);

            if (spriteRenderer == null)
                yield break;
            spriteRenderer.material = originalMaterial;
            yield return new WaitForSeconds(flashDuration);
        }

        flashRoutine = null;
    }
}
