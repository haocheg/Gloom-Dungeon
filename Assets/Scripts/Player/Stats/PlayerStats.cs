using System;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[Serializable]
public class PlayerStats
{
    [Header("»ù´¡ÊôÐÔ")]
    public int maxHp;
    public int Attack;
    public int Defense;

    public bool canDash;
    public bool canDoubleJump;
    public bool canWallSlider;
    public bool canEdgeHang;
    public bool isFirstLoad;

    public PlayerStats()
    {
        maxHp = 100;
        Attack = 10;
        Defense = 10;
        canDash = false;
        canDoubleJump = false;
        canWallSlider = false;
        canEdgeHang = false;
        isFirstLoad = true;
    }
}
