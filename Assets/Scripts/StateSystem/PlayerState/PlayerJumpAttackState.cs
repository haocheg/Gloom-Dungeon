using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerJumpAttackState : PlayerState
{
    public PlayerJumpAttackState(StateMachine machine, string stateName) : base(machine, stateName)
    {
        EventCenter.Instance.AddEventListener<bool>(E_TheEvent.E_PlayerAttackOver, AnimOver);
    }

    private bool isOver = false;

    public override void EnterState()
    {
        isOver = false;
        Controller.PlayAnim("JumpAttack", true);
        Controller.JumpAttack();
    }

    public override void ExitState()
    {
        Controller.PlayAnim("JumpAttack", false);
        Controller.ResetAnim("JumpAttackTrigger");
    }

    public override void Update()
    {
        if (InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.Dash) && Controller.CanDash())
        {
            Controller.ChangeState(Player.PlayerState.DashState);
            return;
        }

        if (InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.Jump) && Controller.TryConsumeDoubleJump())
        {
            Controller.ChangeState(Player.PlayerState.JumpState);
            return;
        }



        if (Controller.isOnGround)
        {
            Controller.PlayAnim("JumpAttackTrigger");
            Controller.SetVelocity(0, 0);
        }

        if (isOver && Controller.isOnGround)
        {
            Controller.ChangeState(Player.PlayerState.IdleState);
            return;
        }

    }

    private void AnimOver(bool isover)
    {
        isOver = isover;
    }

}
