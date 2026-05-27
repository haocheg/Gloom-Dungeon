using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerStats
{
    public int CurHp;
    public int CurAtk;
    public int CurDef;
    public int Score;

    public bool canDash;
    public bool canDoubleJump;
    public bool canWallSlider;
    public bool canEdgeHang;

    public PlayerStats()
    {
        CurHp = 0;
        CurAtk = 0;
        CurDef = 0;
        Score = 0;
        canDash = false;
        canDoubleJump = false;
        canWallSlider = false;
        canEdgeHang = false;
    }

    public PlayerStats(PlayerStats playerStats)
    {
        CurHp = playerStats.CurHp;
        CurAtk = playerStats.CurAtk;
        CurDef = playerStats.CurDef;
        Score = playerStats.Score;
        canDash = playerStats.canDash;
        canDoubleJump = playerStats.canDoubleJump;
        canWallSlider = playerStats.canWallSlider;
        canEdgeHang = playerStats.canEdgeHang;
    }

}
