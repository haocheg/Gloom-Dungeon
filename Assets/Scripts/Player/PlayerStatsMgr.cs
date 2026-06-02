using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerStatsMgr : StatsManager
{
    Player player;
    PlayerStats lastStats;
    PlayerStats curStats;

    public PlayerStatsMgr(Entity entity) : base(entity)
    {
        EventCenter.Instance.AddEventListener(E_TheEvent.E_ChangeScene, OnChangeScene);
    }

    public override void TakeDamage(int damage, Transform damageDealer)
    {
        if (isRespawn) return;
        int curHp = curStats.CurHp;
        curHp -= damage;
        if (curHp <= 0)
        {
            Die();
            return;
        }
        PlayerService.Instance.OnDamageTaken();
        curStats.CurHp = curHp;
        EventCenter.Instance.EventTrigger<int>(E_TheEvent.E_PlayerHealthChange, -damage);
    }

    protected override void Die()
    {
        entity.isDead = true;
        curStats.CurHp = 0;
        PlayerService.Instance.Die();
    }

    public override void Init()
    {
        player = (Player)entity;
        curStats = new PlayerStats();
        lastStats = new PlayerStats();
        curStats.CurHp = player.Stats.maxHp;
        PlayerService.Instance.ApplySkill(curStats);
    }

    public override void Respawn()
    {
        base.Respawn();
        curStats = new PlayerStats(lastStats);
        curStats.CurHp = player.Stats.maxHp;
        PlayerService.Instance.ApplySkill(curStats);
    }

    private void OnChangeScene()
    {
        lastStats = new PlayerStats(curStats);
    }

    public override void Restart()
    {
        curStats = new PlayerStats();
        curStats.CurHp = player.Stats.maxHp;
        lastStats = new PlayerStats();
        PlayerService.Instance.ApplySkill(curStats);
    }

    public override int GetCurHP()
    {
        return curStats.CurHp;
    }

}
