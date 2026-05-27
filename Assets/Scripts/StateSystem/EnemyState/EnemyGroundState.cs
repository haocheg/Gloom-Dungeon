using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EnemyGroundState : EnemyState
{
    public EnemyGroundState(Enemy enemy, StateMachine machine, string stateName) : base(enemy, machine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.Controller.PlayerDetection())
        {
            enemy.Controller.ChangeState(Enemy.EnemyState.BattleState);
            return;
        }

    }

}
