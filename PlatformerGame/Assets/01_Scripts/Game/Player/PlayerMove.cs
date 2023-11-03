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
    
    public bool IsMoving { get; private set; }
    
    void Start()
    {
        playerTransform = transform;
        moveSpeed = walkSpeed;
    }

    public void Move(Vector3 dir)
    {
        SetMoveSpeed();
        
        playerTransform.position += moveSpeed * Time.deltaTime * dir;

        if (dir != Vector3.zero)
        {
            playerCtr.SetPlayerState(PlayerState.Move);
            IsMoving = true;
        }
        else
        {
            playerCtr.SetPlayerState(PlayerState.Idle);
            IsMoving = false;
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
}
