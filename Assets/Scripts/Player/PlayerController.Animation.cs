public partial class PlayerController
{
    public void PlayAnim(string animName, bool trigger)
    {
        animator.SetBool(animName, trigger);
    }

    public void PlayAnim(string animName, int index)
    {
        animator.SetInteger(animName, index);
    }

    public void PlayAnim(string animName)
    {
        animator.SetTrigger(animName);
    }

    public void ResetAnim(string animName)
    {
        animator.ResetTrigger(animName);
    }

    public void JumpOrFallAnim()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
    }
}