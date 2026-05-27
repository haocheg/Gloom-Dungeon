using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string stateName;
    protected float stateTimer;
    protected bool stateActive;

    public EntityState(StateMachine machine, string stateName)
    {
        this.stateMachine = machine;
        this.stateName = stateName;
    }

    public virtual void EnterState()
    {
        //Debug.Log("我进入了 " + stateName);
        stateActive = true;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void ExitState()
    {
        //Debug.Log("我退出了 " + stateName);
        stateActive = false;
    }

}
