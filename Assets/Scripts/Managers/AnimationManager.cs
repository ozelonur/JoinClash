using UnityEngine;
using Ozel;
public enum AnimationState
{
    Idle,
    Running,
    Kick,
    Dance,
    Hit,
    Fly
}

public class AnimationManager : Singleton<AnimationManager>
{
   public void SetAnimation(Animator animator, AnimationState animationState)
    {
        int animState = (int)animationState;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationState.ToString()))
        {
            animator.SetInteger(Constants.ANIM_PLAYER, animState);
        }
    }
}
