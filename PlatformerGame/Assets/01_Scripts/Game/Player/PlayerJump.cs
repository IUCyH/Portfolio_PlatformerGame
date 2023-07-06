using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    Transform feet;

    [SerializeField]
    Rigidbody2D rb;
    
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float overlapCircleRadius;

    int groundLayer;
    
    [SerializeField]
    bool canJump;
    
    public void CheckJump()
    {
        if (PlayerOnGround())
        {
            canJump = true;
        }
    }

    public void Jump()
    {
        if (!canJump) return;
        
        rb.AddForce(jumpForce * Time.fixedDeltaTime * Vector2.up, ForceMode2D.Impulse);
        canJump = false;
    }

    bool PlayerOnGround()
    {
        bool isGround = Physics2D.OverlapCircle(feet.position, overlapCircleRadius, groundLayer);
        Debug.Log(isGround);
        return isGround;
    }

    void Start()
    {
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }
}
