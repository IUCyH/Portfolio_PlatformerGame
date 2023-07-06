using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float runSpeed;
    float moveSpeed;

    public void Move(Vector3 dir)
    {
        SetMoveSpeed();
        
        playerTransform.position += moveSpeed * Time.deltaTime * dir;
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
