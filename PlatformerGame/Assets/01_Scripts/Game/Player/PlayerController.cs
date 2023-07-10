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
    [SerializeField]
    PlayerSkill playerSkill;

    bool stopMovement;
    
    public void StopMovement()
    {
        stopMovement = true;
    }

    public void ContinueMovement()
    {
        stopMovement = false;
    }

    void SetPlayerForward(float dir)
    {
        var dirVector = Vector3.one;

        if (dir != 0f)
        {
            dirVector.x *= dir;
            transform.localScale = dirVector;
        }
    }

    void Update()
    {   
        if (!stopMovement)
        {
            var dir = InputManager.GetAxisRaw(Axis.Horizontal);

            playerMove.Move(Vector3.right * dir);
            SetPlayerForward(dir);
        }

        playerSkill.ExecuteSkills();

        if (InputManager.GetKeyDown(Key.Up))
        {
            playerJump.CheckCanJump();
        }
        playerJump.SetJumpCountToZeroWhenPlayerOnTheGround();
    }

    void FixedUpdate()
    {
        playerJump.Jump();
    }
}
