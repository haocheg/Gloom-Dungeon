using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(StateMachine machine, string stateName) : base(machine, stateName)
    {
    }

    float jumpDur = 0.15f;

    public override void EnterState()
    {
        base.EnterState();
        Controller.WallJump();
        Controller.PlayAnim("JumpOrFall", true);
        stateTimer = jumpDur;
    }

    public override void ExitState()
    {
        base.ExitState();
        Controller.PlayAnim("JumpOrFall", false);
    }

    public override void Update()
    {
        base.Update();
        if (InputMgr.ListenInput(PlayerInputMgr.PlayerInputType.Dash) && Controller.CanDash())
        {
            Controller.ChangeState(Player.PlayerState.DashState);
            return;
        }

        if (InputMgr.ListenInput(PlayerInputMgr.PlayerInputType.Jump) && Controller.TryConsumeDoubleJump())
        {
            Controller.ChangeState(Player.PlayerState.JumpState);
            return;
        }

        if (InputMgr.ListenInput(PlayerInputMgr.PlayerInputType.Move) && stateTimer < 0.0f)
        {
            Controller.Move();
        }

        if (InputMgr.ListenInput(PlayerInputMgr.PlayerInputType.BasicAttack))
        {
            Controller.ChangeState(Player.PlayerState.JumpAttack);
            return;
        }

        if (Controller.Fall())
        {
            Controller.ChangeState(Player.PlayerState.FallState);
            return;
        }
        else if (Controller.isTouchWall && stateTimer < 0.0f && Controller.canWallSlider)
            Controller.ChangeState(Player.PlayerState.WallSlider);
    }

}
