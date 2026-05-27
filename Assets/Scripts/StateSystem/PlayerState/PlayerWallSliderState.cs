using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerWallSliderState : PlayerState
{
    public PlayerWallSliderState(StateMachine machine, string stateName) : base(machine, stateName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Controller.PlayAnim("WallSlider", true);
        Controller.Stop();
    }

    public override void ExitState()
    {
        base.ExitState();
        Controller.PlayAnim("WallSlider", false);
    }

    public override void Update()
    {
        base.Update();
        if (!stateActive)
            return;
        if (InputMgr.ListenInput(PlayerInputMgr.PlayerInputType.WallJump))
        {
            Controller.ChangeState(Player.PlayerState.WallJump);
            return;
        }

        if (InputMgr.ListenInput(PlayerInputMgr.PlayerInputType.Move))
            Controller.WallLeave();
        if (Controller.grabEdge)
            Controller.WallSlider();
        else if (!Controller.isTouchWall && !Controller.isOnGround)
            Controller.ChangeState(Player.PlayerState.FallState);
        else if (Controller.isOnGround)
            Controller.ChangeState(Player.PlayerState.IdleState);
        else
            Controller.WallSlider();
    }

}
