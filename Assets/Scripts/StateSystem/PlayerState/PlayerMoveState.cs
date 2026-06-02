using System.Collections;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(StateMachine machine, string stateName) : base(machine, stateName)
    {
    }

    float moveDur = 0.1f;

    public override void EnterState()
    {
        base.EnterState();
        stateTimer = moveDur;
        Controller.PlayAnim("Move", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        Controller.PlayAnim("Move", false);
    }

    public override void Update()
    {
        base.Update();
        if (!stateActive)
            return;
        Controller.Move();
        if (!InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.Move) || Controller.isTouchWall)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer < 0)
            {
                Controller.ChangeState(Player.PlayerState.IdleState);
                return;
            }
        }
        else
            stateTimer = moveDur;
        if (InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.Dash) && Controller.CanDash())
        {
            Controller.ChangeState(Player.PlayerState.DashState);
            return;
        }

        if (InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.BasicAttack))
        {
            Controller.ChangeState(Player.PlayerState.BasicAttack);
            return;
        }
    }

}
