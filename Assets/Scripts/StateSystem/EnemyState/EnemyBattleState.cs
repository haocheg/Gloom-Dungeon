using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EnemyBattleState : EnemyState
{
    public EnemyBattleState(Enemy enemy, StateMachine machine, string stateName) : base(enemy, machine, stateName)
    {
    }



    public override void EnterState()
    {
        base.EnterState();
        enemy.Controller.Retreat();
    }

    public override void ExitState()
    {
        base.ExitState();
    }


    public override void Update()
    {
        base.Update();
        if (!stateActive) return;
        if (!enemy.Controller.FindTarget())
        {
            enemy.Controller.ChangeState(Enemy.EnemyState.IdleState);
            return;
        }

        if (enemy.Controller.InAttackRange())
        {
            enemy.Controller.ChangeState(Enemy.EnemyState.AttackState);
            return;
        }
        else
        {
            enemy.Controller.MoveToTarget();
        }

    }

}
