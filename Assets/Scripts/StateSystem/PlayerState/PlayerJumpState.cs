using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(StateMachine machine, string stateName) : base(machine, stateName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Controller.Jump();
        Controller.PenetratePlatform(0.5f);
    }

    public override void Update()
    {
        base.Update();
        if (!stateActive)
            return;
        if (Controller.Fall())
        {
            Controller.ChangeState(Player.PlayerState.FallState);
            return;
        }
    }

}
