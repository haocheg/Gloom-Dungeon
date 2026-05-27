using System.Net.NetworkInformation;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EntityStatsManager : Singleton<EntityStatsManager>
{
    private EntityStatsManager()
    {

    }

    public void SaveData(EntityStats stats, string fileName)
    {
        JsonMgr.Instance.SaveData(stats, fileName);
    }

    public EntityStats LoadData(string fileName)
    {
        return JsonMgr.Instance.LoadData<EntityStats>(fileName);
    }


    public void TakeDamage(Entity target, int damage, Transform damageDealer)
    {
        if (target.isDead) return;
        ApplyDamage(target, damage, damageDealer);
    }

    private void ApplyDamage(Entity target, int damage, Transform damageDealer)
    {
        target.StatsManager.TakeDamage(damage, damageDealer);
    }

}
