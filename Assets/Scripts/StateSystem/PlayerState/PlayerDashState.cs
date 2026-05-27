using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerDashState : PlayerState
{
    public PlayerDashState(StateMachine machine, string stateName) : base(machine, stateName)
    {
    }

    float dashDur = 0.25f;

    public override void EnterState()
    {
        base.EnterState();
        Controller.PlayAnim("Dash", true);
        stateTimer = dashDur;
        Controller.DashStart();
    }

    public override void ExitState()
    {
        base.ExitState();
        Controller.PlayAnim("Dash", false);
        Controller.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();
        if (Controller.isTouchWall)
        {
            if (Controller.isOnGround)
                Controller.ChangeState(Player.PlayerState.IdleState);
            else if (Controller.canWallSlider)
                Controller.ChangeState(Player.PlayerState.WallSlider);
            return;
        }
        if (stateTimer < 0.0f)
        {
            if (Controller.isOnGround)
                Controller.ChangeState(Player.PlayerState.IdleState);
            else
                Controller.ChangeState(Player.PlayerState.FallState);
            return;
        }
        Controller.Dash();
    }

}
