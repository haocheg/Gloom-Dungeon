using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(StateMachine machine, string stateName) : base(machine, stateName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Controller.PlayAnim("Dead", true);
        Controller.Stop();
        Controller.Death();
    }

    public override void ExitState()
    {
        base.ExitState();
        Controller.PlayAnim("Dead", false);
    }

}
