using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class Enemy : Entity
{
    [SerializeField] private EnemyController controller;
    public EnemyController Controller { get => controller; }
    public enum EnemyState
    {
        IdleState,
        MoveState,
        AttackState,
        BattleState,
        DeadState,
    }

    protected override void Awake()
    {
        base.Awake();
        Stats = new EntityStats(Stats.maxHp, Stats.Attack);
        StatsManager = new EnemyStatsMgr(this);
    }

    protected override void Update()
    {
        base.Update();
    }

}
