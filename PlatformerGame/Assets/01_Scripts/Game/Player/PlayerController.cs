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
        var dir = Input.GetAxis("Horizontal");
        playerMove.Move(Vector3.right * dir);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerJump.CheckJump();
        }
    }

    void FixedUpdate()
    {
        playerJump.Jump();
    }
}
