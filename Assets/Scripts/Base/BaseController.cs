using UnityEngine;

public class BaseController : MonoBehaviour
{
    private AnimationManager animationManager;
    private Animator animator;

    protected ParticleManager particleManager;
    // Start is called before the first frame update
    protected  virtual void Start()
    {
        animator = GetComponent<Animator>();
        animationManager = AnimationManager.Instance;
        particleManager = ParticleManager.Instance;
    }

    protected virtual void Idle()
    {
        animationManager.SetAnimation(animator, AnimationState.Idle);
    }
    protected virtual void RunAnim()
    {
        animationManager.SetAnimation(animator, AnimationState.Running);
    }

    protected virtual void Kick()
    {
        animationManager.SetAnimation(animator, AnimationState.Kick);
    }

    protected virtual void Dance()
    {
        animationManager.SetAnimation(animator, AnimationState.Dance);
    }

    protected void Hit()
    {
        animationManager.SetAnimation(animator, AnimationState.Hit);
    }


    protected virtual void Dead()
    {
        particleManager.RunSmashParticle(transform);
        Destroy(gameObject);
    }
}
