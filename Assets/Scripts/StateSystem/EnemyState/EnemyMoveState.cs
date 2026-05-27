using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EnemyMoveState : EnemyGroundState
{
    public EnemyMoveState(Enemy enemy, StateMachine machine, string stateName) : base(enemy, machine, stateName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        if (!enemy.Controller.thereHasGround || enemy.Controller.isTouchWall)
            enemy.Controller.Flip();
    }

    public override void Update()
    {
        base.Update();
        if (!stateActive)
            return;
        enemy.Controller.Move();
        if (!enemy.Controller.thereHasGround || enemy.Controller.isTouchWall)
        {
            enemy.Controller.ChangeState(Enemy.EnemyState.IdleState);
            return;
        }
    }

}
