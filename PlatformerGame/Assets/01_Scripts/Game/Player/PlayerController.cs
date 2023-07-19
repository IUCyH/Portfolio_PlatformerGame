using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Idle,
    Move,
    Attack,
    Hit,
    Die,
    Max
}

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

    PlayerState playerState = PlayerState.Idle;
    PlayerState prevState = PlayerState.Idle;

    bool stopMovement;

    public void SetPlayerState(PlayerState state)
    {
        playerState = state;
    }

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
    
    void PlayAnimation(PlayerAnimations motion)
    {
        playerAnimation.PlayAnimation(motion);
    }

    void PlayAnimationByPlayerState()
    {
        if (playerState == prevState) return;

        switch (playerState)
        {
            case PlayerState.Idle:
                PlayAnimation(PlayerAnimations.Idle);
                break;
            case PlayerState.Move:
                PlayAnimation(PlayerAnimations.Move);
                break;
            case PlayerState.Attack:
                PlayAnimation(PlayerAnimations.Attack);
                break;
            case PlayerState.Hit:
                PlayAnimation(PlayerAnimations.Hit);
                break;
            case PlayerState.Die:
                PlayAnimation(PlayerAnimations.Die);
                break;
        }

        prevState = playerState;
    }

    void Update()
    {
        if (!stopMovement)
        {
            var dir = InputManager.GetAxisRaw(Axis.Horizontal);

            playerMove.Move(Vector3.right * dir);
            SetPlayerForward(dir);
        }

        if (InputManager.GetKeyDown(Key.Up))
        {
            Debug.Log("JUMP KEY GET");
            playerJump.CheckCanJump();
        }
        playerJump.SetJumpCountToZeroWhenPlayerOnTheGround();

        playerSkill.ExecuteSkills();
        
        PlayAnimationByPlayerState();
    }

    void FixedUpdate()
    {
        playerJump.Jump();
    }
}
