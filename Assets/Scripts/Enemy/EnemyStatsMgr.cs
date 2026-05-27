using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EnemyStatsMgr : StatsManager
{
    Enemy enemy;
    public EnemyStatsMgr(Entity entity) : base(entity)
    {
        enemy = (Enemy)entity;
    }

    public override void TakeDamage(int damage, Transform damageDealer)
    {
        base.TakeDamage(damage, damageDealer);
        //enemy = (Enemy)entity;
        enemy.Controller.GetDamage(damageDealer);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Controller.Die();
    }

    public override void Respawn()
    {
        base.Respawn();
        Enemy enemy = (Enemy)entity;
        CurrentHP = enemy.Stats.maxHp;
    }

}
