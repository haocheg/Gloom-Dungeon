using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EnemyState : EntityState
{
    protected Enemy enemy;
    protected PoolManager Pool { get { return PoolManager.Instance; } }
    public EnemyState(Enemy enemy, StateMachine machine, string stateName) : base(machine, stateName)
    {
        this.enemy = enemy;
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.animator.SetBool(stateName, true);
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.animator.SetBool(stateName, false);
    }

}
