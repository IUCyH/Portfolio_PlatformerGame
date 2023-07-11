using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI jumpCountText;
    [SerializeField]
    Image skillGaugeImg;
    
    [Header("")]
    [SerializeField]
    PlayerMove playerMove;
    [SerializeField]
    PlayerJump playerJump;
    [SerializeField]
    PlayerSkill playerSkill;

    bool stopMovement;

    public void UpdateJumpCountText(int count)
    {
        PlayerUIManager.Instance.UpdateJumpCountText(jumpCountText, count);
    }

    public void UpdateSkillGauge(float value)
    {
        PlayerUIManager.Instance.UpdateSkillGauge(skillGaugeImg, value);
    }
    
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
