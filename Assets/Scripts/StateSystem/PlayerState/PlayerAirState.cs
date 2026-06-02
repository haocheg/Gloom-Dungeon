using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerAirState : PlayerState
{
    public PlayerAirState(StateMachine machine, string stateName) : base(machine, stateName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Controller.PlayAnim("JumpOrFall", true);

    }

    public override void ExitState()
    {
        base.ExitState();
        Controller.PlayAnim("JumpOrFall", false);

    }

    public override void Update()
    {
        base.Update();
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

        if (InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.BasicAttack))
        {
            Controller.ChangeState(Player.PlayerState.JumpAttack);
            return;
        }

        if (InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.Move))
            Controller.Move();
        Controller.JumpOrFallAnim();
    }

}
