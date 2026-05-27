using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class StateMachine
{
    public EntityState currentState { get; private set; }
    public Entity entity { get; private set; }

    public bool canChangeState = true;

    public void Init(EntityState entityState, Entity entity)
    {
        currentState = entityState;
        this.entity = entity;
        currentState.EnterState();
    }

    public void ChangeState(EntityState entityState)
    {
        if (!canChangeState) return;
        currentState?.ExitState();
        currentState = entityState;
        currentState.EnterState();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }

    public void SwitchOffStateMachine() => canChangeState = false;

}
