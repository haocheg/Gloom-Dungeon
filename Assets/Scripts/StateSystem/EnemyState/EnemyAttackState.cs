using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, StateMachine machine, string stateName) : base(enemy, machine, stateName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Controller.Attack();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        base.Update();
        if (!stateActive) return;

        if (!enemy.Controller.isAttacking)
        {
            enemy.Controller.ChangeState(Enemy.EnemyState.BattleState);
            return;
        }

        if (enemy.Controller.thereHasGround)
            enemy.Controller.Stop();

    }


}
