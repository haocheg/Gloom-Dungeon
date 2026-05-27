using System.Collections;
using System.Diagnostics;
using System.Net.NetworkInformation;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public abstract class StatsManager
{
    public Entity entity;
    public int CurrentHP;
    protected bool isRespawn;
    private float respawnDur = 3.0f;

    public StatsManager(Entity entity)
    {
        this.entity = entity;
    }

    public virtual void TakeDamage(int damage, Transform damageDealer)
    {
        if (isRespawn) return;
        int curHp = CurrentHP;
        curHp -= damage;
        if (curHp <= 0)
        {
            Die();
            return;
        }
        CurrentHP = curHp;
    }

    protected virtual void Die()
    {
        entity.isDead = true;
        CurrentHP = 0;
    }

    public virtual void Init()
    {

    }

    public virtual void Respawn()
    {
        isRespawn = true;
        MonoMgr.Instance.StartCoroutine(AtRespawn());
    }

    IEnumerator AtRespawn()
    {
        yield return new WaitForSeconds(respawnDur);
        isRespawn = false;
    }

    public virtual void Restart()
    {

    }

    public virtual int GetCurHP()
    {
        return CurrentHP;
    }

}
