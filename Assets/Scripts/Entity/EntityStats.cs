using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[System.Serializable]
public class EntityStats
{
    [Header("»ù´¡ÊôĞÔ")]
    public int maxHp;
    public int Attack;
    public int Defense;

    public EntityStats()
    {

    }


    public EntityStats(int hp, int atk = 1, int def = 1)
    {
        maxHp = hp;
        Attack = atk;
        Defense = def;
    }

}
