using UnityEngine;

public partial class PlayerController
{
    public void Die()
    {
        if (player.isDead)
            ChangeState(Player.PlayerState.DeadState);
    }

    public void Death()
    {
        input.Disable();
        player.gameObject.layer = LayerMask.NameToLayer("PlayerDeath");
        platformCollider.gameObject.layer = LayerMask.NameToLayer("PlayerDeath");
    }

    public void Respawn()
    {
        player.isDead = false;
        player.gameObject.layer = LayerMask.NameToLayer("Player");
        platformCollider.gameObject.layer = LayerMask.NameToLayer("Player");
        player.StatsManager.Respawn();
        ResetAirJumpCount();
        input.Enable();
        ChangeState(Player.PlayerState.IdleState);
    }

    public void Restart()
    {
        player.isDead = false;
        player.gameObject.layer = LayerMask.NameToLayer("Player");
        platformCollider.gameObject.layer = LayerMask.NameToLayer("Player");
        player.StatsManager.Restart();
        canDash = false;
        canDoubleJump = false;
        canWallSlider = false;
        canEdgeHang = false;
        ResetAirJumpCount();
        input.Enable();
        ChangeState(Player.PlayerState.IdleState);
    }

    public void InitPlayer()
    {
        player.isDead = false;
        player.StatsManager.Init();
        ResetAirJumpCount();
        ChangeState(Player.PlayerState.IdleState);
    }

    public int GetHealthValue()
    {
        return player.StatsManager.GetCurHP();
    }

    public void ShowOrHide(bool isShow)
    {
        animator.enabled = isShow;
        player.gameObject.SetActive(isShow);
        rb.gravityScale = isShow ? gravityScale : 0f;
    }

    public void EnableOrDisableInput(bool isEnable)
    {
        if (isEnable)
        {
            input.Player.Enable();
        }
        else
        {
            input.Player.Disable();
        }
    }

    public void KeepPlayer()
    {
        DontDestroyOnLoad(player);
    }

    public void GetSkill(PlayerCurrentStats stats)
    {
        canDash = stats.canDash;
        canDoubleJump = stats.canDoubleJump;
        canWallSlider = stats.canWallSlider;
        canEdgeHang = stats.canEdgeHang;
        ResetAirJumpCount();
    }
}
