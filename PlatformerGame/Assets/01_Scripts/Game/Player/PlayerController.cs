using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    const float RightRotation = 0f;
    const float LeftRotation = 180f;
    
    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI[] skillCooldownTexts;
    [SerializeField]
    TextMeshProUGUI jumpCountText;
    [SerializeField]
    Image skillGaugeImg;

    [Header("")]
    [SerializeField]
    PlayerAnimation playerAnimation;
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
        PlayerUIManager.Instance.UpdateFillAmountOfSkillGaugeImage(skillGaugeImg, value);
    }

    public void UpdateSkillCooldownText(Skills skill, float cooldown)
    {
        float result = (float)Math.Round(cooldown, 2);
        PlayerUIManager.Instance.UpdateSkillCooldownText(skillCooldownTexts[(int)skill], result);
    }

    public void PlayAnimation(PlayerAnimations motion)
    {
        playerAnimation.PlayAnimation(motion);
    }

    public void BackToIdleAnimation()
    {
        playerAnimation.BackToIdleAnimation();
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
        if (dir != 0f)
        {
            var playerForward = dir > 0 ? RightRotation : LeftRotation;

            var playerXRotation = transform.rotation.eulerAngles.x;
            var playerZRotation = transform.rotation.eulerAngles.z;

            transform.rotation = Quaternion.Euler(playerXRotation, playerForward, playerZRotation);
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
