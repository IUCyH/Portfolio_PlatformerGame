using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    PlayerController playerCtr;
    Transform playerTransform;
    
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float runSpeed;
    float moveSpeed;

    bool playingWalkingAnim;

    public void Move(Vector3 dir)
    {
        SetMoveSpeed();
        
        playerTransform.position += moveSpeed * Time.deltaTime * dir;

        playingWalkingAnim = dir != Vector3.zero && playingWalkingAnim;

        if (dir != Vector3.zero)
        {
            playerCtr.SetPlayerState(PlayerState.Move);
        }
        else
        {
            playerCtr.SetPlayerState(PlayerState.Idle);
        }
    }

    void SetMoveSpeed()
    {
        if (InputManager.GetKey(Key.Run))
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

    void Start()
    {
        playerTransform = transform;
        moveSpeed = walkSpeed;
    }
}
