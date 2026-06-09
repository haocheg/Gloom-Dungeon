using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerCurrentStats
{
    public int CurHp;
    public int CurAtk;
    public int CurDef;

    public bool canDash;
    public bool canDoubleJump;
    public bool canWallSlider;
    public bool canEdgeHang;

    public PlayerCurrentStats()
    {
        CurHp = 0;
        CurAtk = 0;
        CurDef = 0;
        canDash = false;
        canDoubleJump = false;
        canWallSlider = false;
        canEdgeHang = false;
    }

    public PlayerCurrentStats(PlayerCurrentStats playerStats)
    {
        CurHp = playerStats.CurHp;
        CurAtk = playerStats.CurAtk;
        CurDef = playerStats.CurDef;
        canDash = playerStats.canDash;
        canDoubleJump = playerStats.canDoubleJump;
        canWallSlider = playerStats.canWallSlider;
        canEdgeHang = playerStats.canEdgeHang;
    }

    public PlayerCurrentStats(PlayerStats playerStats)
    {
        CurHp = playerStats.maxHp;
        CurAtk = playerStats.Attack;
        CurDef = playerStats.Defense;
        canDash = playerStats.canDash;
        canDoubleJump = playerStats.canDoubleJump;
        canWallSlider = playerStats.canWallSlider;
        canEdgeHang = playerStats.canEdgeHang;
    }

}
