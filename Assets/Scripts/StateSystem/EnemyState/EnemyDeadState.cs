using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(Enemy enemy, StateMachine machine, string stateName) : base(enemy, machine, stateName)
    {
    }

    public override void EnterState()
    {
        enemy.Controller.Death();
        Pool.PushGameObject(enemy.gameObject, false, 5.0f);
    }

}
