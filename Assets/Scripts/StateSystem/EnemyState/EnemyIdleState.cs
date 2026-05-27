using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EnemyIdleState : EnemyGroundState
{
    public EnemyIdleState(Enemy enemy, StateMachine machine, string stateName) : base(enemy, machine, stateName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Controller.Stop();
        stateTimer = enemy.Controller.idleDur;
    }

    public override void Update()
    {
        base.Update();
        if (!stateActive) return;
        if (stateTimer < 0)
        {
            enemy.Controller.ChangeState(Enemy.EnemyState.MoveState);
            return;
        }
    }

}
