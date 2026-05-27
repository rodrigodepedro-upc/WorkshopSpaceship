using UnityEngine;

/// <summary>
/// Plays the shield's "animation" state on demand, ignoring the request while it
/// is still playing so it doesn't restart or stack mid-play.
///
/// The "still playing" guard uses an internal flag rather than querying the
/// Animator: Animator.Play is deferred until the next animation evaluation, so
/// GetCurrentAnimatorStateInfo would still report the old state for calls made
/// in the same frame — letting simultaneous hits trigger it twice.
///
/// The flag is cleared by <see cref="ShieldAnimationStateBehaviour"/>, which
/// must be added to the "animation" state in the Animator Controller so the
/// flag reacts to the animation actually finishing.
/// </summary>
[RequireComponent(typeof(Animator))]
public class ShieldAnimation : MonoBehaviour
{
    const string AnimationName = "animation";

    Animator animator = null;
    bool isPlaying = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnDisable()
    {
        // The shield is disabled when its health hits zero, which can interrupt
        // the animation state before it exits — leaving isPlaying stuck true and
        // blocking every future PlayAnimation. Re-arm here so it plays again once
        // the shield is regenerated and re-enabled.
        isPlaying = false;
    }

    public void PlayAnimation()
    {
        if (isPlaying)
            return;

        isPlaying = true;
        animator.Play(AnimationName);
    }

    /// <summary>Called by the state behaviour when the animation state exits.</summary>
    public void OnAnimationFinished()
    {
        isPlaying = false;
    }
}
