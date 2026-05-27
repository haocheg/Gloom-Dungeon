using UnityEngine;

public partial class PlayerController
{
    public void TeleportPlayer(Vector3 pos)
    {
        player.transform.position = pos;
    }

    public void Move()
    {
        SetVelocity(moveInput.x * moveSpeed, rb.velocity.y);
    }

    public void Stop()
    {
        SetVelocity();
        dashTime = 1;
    }

    public void Flip()
    {
        player.transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    public void Jump()
    {
        SetVelocity(rb.velocity.x, jumpForce);
    }

    public void ResetAirJumpCount()
    {
        remainingAirJumpCount = canDoubleJump ? maxAirJumpCount : 0;
    }

    public bool TryConsumeDoubleJump()
    {
        if (!canDoubleJump || remainingAirJumpCount <= 0 || isOnGround)
            return false;

        remainingAirJumpCount--;
        return true;
    }

    public bool Fall()
    {
        return rb.velocity.y < 0;
    }

    public void WallSlider()
    {
        sliderInput = input.Player.WallSlider.ReadValue<Vector2>();
        if (sliderInput.y < 0)
            SetVelocity(0, rb.velocity.y);
        else
            SetVelocity(0, rb.velocity.y * 0.85f);
    }

    public void WallLeave()
    {
        if (moveInput.x != facingDir)
            SetVelocity(moveInput.x * moveSpeed, rb.velocity.y);
    }

    public void WallJump()
    {
        SetVelocity(wallJumpDir.x * -facingDir, wallJumpDir.y);
    }

    public void Dash()
    {
        SetVelocity(dashSpeed * facingDir, 0);
        if (!isTouchWall && !isOnGround)
            dashTime = 0;
    }

    public void DashStart()
    {
        player.gameObject.layer = LayerMask.NameToLayer("PlayerDash");
    }

    public void DashOver()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public bool CanDash()
    {
        if (!canDash)
            return false;
        if (isTouchWall)
            return false;
        if (stateMachine.currentState == playerStates[Player.PlayerState.DashState])
            return false;
        if (dashTime == 0)
            return false;
        if (dashCoolTime > 0)
            return false;

        dashCoolTime = 0.5f;
        return true;
    }

    public void JumpAttack()
    {
        SetVelocity(rb.velocity.x, -10.0f);
    }

    public void AttackMove(int index)
    {
        float attackDir = moveInput.x == 0 ? facingDir : moveInput.x;
        SetVelocity(attackVelocity[index - 1] * attackDir, rb.velocity.y);
    }

    public void SetVelocity(float xMultiplier = 0, float yMultiplier = 0)
    {
        rb.velocity = new Vector2(xMultiplier, yMultiplier);
        if (rb.velocity.x > 3.0f && !facingRight)
            Flip();
        else if (rb.velocity.x < -3.0f && facingRight)
            Flip();
    }

    public void Hang(bool isHang)
    {
        if (isHang)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    public void PenetratePlatform(float ignoreDur)
    {
        ignorePlatformTimer = ignoreDur;
    }
}
