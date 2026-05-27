using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerEdgeHangState : PlayerState
{
    public PlayerEdgeHangState(StateMachine machine, string stateName) : base(machine, stateName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Controller.PlayAnim("EdgeGrab", true);
        Controller.Hang(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        Controller.PlayAnim("EdgeGrab", false);
        Controller.Hang(false);
    }

    public override void Update()
    {
        base.Update();

        if (InputMgr.ListenInput(PlayerInputMgr.PlayerInputType.Move))
            Controller.WallLeave();

        if (InputMgr.ListenInput(PlayerInputMgr.PlayerInputType.Jump))
            Controller.ChangeState(Player.PlayerState.JumpState);
        else if (InputMgr.ListenInput(PlayerInputMgr.PlayerInputType.WallSlider) && Controller.canWallSlider)
            Controller.ChangeState(Player.PlayerState.WallSlider);
        else if (!Controller.isOnGround && !Controller.grabEdge)
            Controller.ChangeState(Player.PlayerState.FallState);
    }


}
