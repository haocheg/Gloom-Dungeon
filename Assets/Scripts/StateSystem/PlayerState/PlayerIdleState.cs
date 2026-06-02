using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(StateMachine machine, string stateName) : base(machine, stateName)
    {

    }



    public override void EnterState()
    {
        base.EnterState();

        Controller.Stop();
        Controller.PlayAnim("Idle", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        Controller.PlayAnim("Idle", false);
    }

    public override void Update()
    {
        base.Update();
        if (!stateActive)
            return;
        if (InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.Dash) && Controller.CanDash())
        {
            Controller.ChangeState(Player.PlayerState.DashState);
            return;
        }

        if (InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.DownPlatform))
        {
            Controller.PenetratePlatform(0.2f);
            Controller.ChangeState(Player.PlayerState.FallState);
            return;
        }


        if (Controller.grabEdge && Controller.canEdgeHang)
        {
            Controller.ChangeState(Player.PlayerState.EdgeHang);
            return;
        }


        if (InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.BasicAttack))
        {
            Controller.ChangeState(Player.PlayerState.BasicAttack);
            return;
        }

        if (InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.Move))
        {
            if (Controller.isTouchWall && Controller.moveInput.x == Controller.facingDir)
                return;
            Controller.ChangeState(Player.PlayerState.MoveState);
        }
    }

}
