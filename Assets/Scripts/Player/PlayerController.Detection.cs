using UnityEngine;

public partial class PlayerController
{
    private void HandleDetec()
    {
        isOnGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDis, groundLayer) ||
            (Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDis, platformLayer) && !isIgnoringPlatform);

        isTouchWall = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDis, groundLayer) &&
            Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDis, groundLayer);

        grabEdge = !Physics2D.Raycast(thirdWallCheck.position, Vector2.right * facingDir, wallCheckDis, groundLayer) &&
            Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDis, groundLayer);
    }

    private void RefreshPlatformPenetrate()
    {
        ignorePlatformTimer -= Time.fixedDeltaTime;
        bool cond1 = ignorePlatformTimer > 0;
        bool cond2 = rb.velocity.y > 3.0f;

        if (cond1)
        {
            if (!isIgnoringPlatform)
            {
                player.gameObject.layer = LayerMask.NameToLayer("PlayerIgnorePlatform");
                isIgnoringPlatform = true;
            }
        }
        else if (cond2)
        {
            ignorePlatformTimer = 0.1f;
        }
        else
        {
            bool isInsidePlatform = platformCollider.IsTouchingLayers(platformLayer);
            if (!isInsidePlatform)
            {
                if (isIgnoringPlatform)
                {
                    player.gameObject.layer = LayerMask.NameToLayer("Player");
                    isIgnoringPlatform = false;
                }
            }
            else
            {
                ignorePlatformTimer = 0.02f;
            }
        }
    }
}