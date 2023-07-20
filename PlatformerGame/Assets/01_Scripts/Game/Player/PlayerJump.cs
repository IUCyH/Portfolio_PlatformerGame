using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    PlayerController playerCtr;
    [SerializeField]
    Transform feet;
    [SerializeField]
    Rigidbody2D rb;
    
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float overlapCircleRadius;
    [SerializeField]
    int maxJumpCount;
    [SerializeField]
    int jumpCount;
    int groundLayer;
    [SerializeField]
    bool canJump;

    void Start()
    {
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        //Debug.unityLogger.logEnabled = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(feet.position, overlapCircleRadius);
        //Gizmos.DrawSphere();
    }
    
    public void CheckCanJump()
    {
        if (jumpCount < maxJumpCount)
        {
            canJump = true;
        }
    }

    public void Jump()
    {
        if (!canJump) return;

        rb.velocity = Vector2.zero;
        rb.AddForce(jumpForce * Time.fixedDeltaTime * Vector2.up, ForceMode2D.Impulse);
        
        jumpCount++;
        canJump = false;
    }

    public void SetJumpCountToZeroWhenPlayerOnTheGround()
    {
        var ground = PlayerOnGround();
        if (ground)
        {
            jumpCount = 0;
        }
        playerCtr.UpdateJumpCountText(jumpCount);
    }

    bool PlayerOnGround()
    {
        bool isGround = Physics2D.OverlapCircle(feet.position, overlapCircleRadius, groundLayer);
        return isGround;
    }
}
