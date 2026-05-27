using UnityEngine;

/// <summary>
/// Add this to the shield's "animation" state in the Animator Controller
/// (Add Behaviour in the state inspector). It tells the <see cref="ShieldAnimation"/>
/// on the animated object when the state finishes, so it can re-arm.
/// </summary>
public class ShieldAnimationStateBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ShieldAnimation shield = animator.GetComponent<ShieldAnimation>();

        if (shield != null)
            shield.OnAnimationFinished();
    }
}
