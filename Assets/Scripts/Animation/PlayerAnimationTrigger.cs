using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerAnimationTrigger : MonoBehaviour
{

    public void AttackOver()
    {
        EventCenter.Instance.EventTrigger<bool>(E_TheEvent.E_PlayerAttackOver, true);
    }

    public void AttackTrigger()
    {
        PlayerService.Instance.AttackTrigger();
    }

    public void Death()
    {
        EventCenter.Instance.EventTrigger<bool>(E_TheEvent.E_PlayerDeath, true);
    }

    public void DashOver()
    {
        PlayerService.Instance.DashOver();
    }

}
