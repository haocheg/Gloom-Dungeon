using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerStatsMgr : StatsManager
{
    Player player;
    PlayerCurrentStats lastStats;
    PlayerCurrentStats curStats;
    PlayerStats playerStats;



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
        float hpPercent = (float)curHp / playerStats.maxHp;
        EventCenter.Instance.EventTrigger<float>(E_TheEvent.E_GetPlayerHPPercent, hpPercent);
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
        LoadStats();
        lastStats = new PlayerCurrentStats();
        PlayerService.Instance.ApplySkill(curStats);
    }

    public override void Respawn()
    {
        base.Respawn();
        curStats = new PlayerCurrentStats(lastStats);
        curStats.CurHp = playerStats.maxHp;
        PlayerService.Instance.ApplySkill(curStats);
    }

    private void OnChangeScene()
    {
        lastStats = new PlayerCurrentStats(curStats);
    }

    public override void Restart()
    {
        curStats = new PlayerCurrentStats();
        curStats.CurHp = playerStats.maxHp;
        lastStats = new PlayerCurrentStats();
        PlayerService.Instance.ApplySkill(curStats);
    }

    public override int GetCurHP()
    {
        return curStats.CurHp;
    }

    public void LoadStats()
    {
        PlayerStats loadstats = JsonMgr.Instance.LoadData<PlayerStats>("PlayerStats");
        if (loadstats.isFirstLoad)
        {
            loadstats.isFirstLoad = false;
            curStats = new PlayerCurrentStats(loadstats);
            playerStats = loadstats;
            SaveStats(loadstats);
        }
        playerStats = loadstats;
        curStats = new PlayerCurrentStats(playerStats);
    }

    public void SaveStats(PlayerStats saveStats)
    {
        JsonMgr.Instance.SaveData(saveStats, "PlayerStats");
    }


}
