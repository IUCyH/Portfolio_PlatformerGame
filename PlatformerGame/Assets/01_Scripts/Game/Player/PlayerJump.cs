using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI jumpCountText;
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

    public void CheckCanJump()
    {
        if (jumpCount < maxJumpCount)
        {
            Debug.Log("Jump Key Pressed");
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
            Debug.Log("IT'S TRUE!");
            jumpCount = 0;
        }
        jumpCountText.text = string.Format("Jump Count : {0}", jumpCount); //Just test
    }

    bool PlayerOnGround()
    {
        bool isGround = Physics2D.OverlapCircle(feet.position, overlapCircleRadius, groundLayer);
        return isGround;
    }

    void Start()
    {
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(feet.position, overlapCircleRadius);
        //Gizmos.DrawSphere();
    }
}
