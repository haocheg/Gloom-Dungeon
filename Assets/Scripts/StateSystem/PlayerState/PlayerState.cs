using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerState : EntityState
{
    protected PlayerController Controller { get { return PlayerController.Instance; } }
    protected PlayerInputMgr InputMgr { get { return PlayerInputMgr.Instance; } }

    public PlayerState(StateMachine machine, string stateName) : base(machine, stateName)
    {
    }
}