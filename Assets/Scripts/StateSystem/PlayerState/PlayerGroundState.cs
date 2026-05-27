using System.Collections;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(StateMachine machine, string stateName) : base(machine, stateName)
    {
    }

    float coyoteDur = 0.05f;
    float coyoteTimer;

    public override void EnterState()
    {
        base.EnterState();
        coyoteTimer = coyoteDur;
        Controller.ResetAirJumpCount();
    }

    public override void ExitState()
    {
        base.ExitState();
    }


    public override void Update()
    {
        //base.Update();
        InputMgr.RecordInput(PlayerInputMgr.PlayerInputType.Jump);

        if (InputMgr.ListenInput(PlayerInputMgr.PlayerInputType.Jump))
        {
            Controller.ChangeState(Player.PlayerState.JumpState);
            return;
        }

        if (Controller.Fall() && !Controller.isOnGround)
        {
            coyoteTimer -= Time.deltaTime;
            if (coyoteTimer > 0)
            {
                if (InputMgr.CheckInput(PlayerInputMgr.PlayerInputType.Jump))
                {
                    Controller.ChangeState(Player.PlayerState.JumpState);
                    return;
                }
            }
            else
                Controller.ChangeState(Player.PlayerState.FallState);
        }
    }

}
