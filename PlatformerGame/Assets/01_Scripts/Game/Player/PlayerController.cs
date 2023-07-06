using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerMove playerMove;
    [SerializeField]
    PlayerJump playerJump;

    void Update()
    {
        var dir = InputManager.GetAxisRaw(Axis.Horizontal);
        playerMove.Move(Vector3.right * dir);

        if (InputManager.GetKeyDown(Key.Up))
        {
            playerJump.CheckJump();
        }
    }

    void FixedUpdate()
    {
        playerJump.Jump();
    }
}
