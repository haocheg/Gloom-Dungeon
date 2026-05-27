using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(StateMachine machine, string stateName) : base(machine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();
        if (!stateActive)
            return;

        if (Controller.grabEdge && Controller.canEdgeHang)
        {
            Controller.ChangeState(Player.PlayerState.EdgeHang);
            return;
        }

        InputMgr.RecordInput(PlayerInputMgr.PlayerInputType.Jump, 0.15f);

        if (Controller.isOnGround)
        {
            if (InputMgr.CheckInput(PlayerInputMgr.PlayerInputType.Jump))
            {
                Controller.ChangeState(Player.PlayerState.JumpState);
                return;
            }
            Controller.ChangeState(Player.PlayerState.IdleState);
        }
        else if (Controller.isTouchWall && Controller.canWallSlider)
            Controller.ChangeState(Player.PlayerState.WallSlider);

    }

}
