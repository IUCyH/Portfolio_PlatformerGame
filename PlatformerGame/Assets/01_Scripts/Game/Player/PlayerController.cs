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
    const float RightYRotationValue = 0f;
    const float LeftYRotationValue = 180f;

    [SerializeField]
    PlayerAnimation playerAnimation;
    [SerializeField]
    PlayerMove playerMove;
    [SerializeField]
    PlayerJump playerJump;
    [SerializeField]
    PlayerSkill playerSkill;
    Transform playerTransform;
    
    PlayerState playerState = PlayerState.Idle;
    PlayerState prevState = PlayerState.Idle;
    bool stopMovement;

    void Start()
    {
        playerTransform = transform;
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
    
    public void SetPlayerState(PlayerState state)
    {
        //플레이어가 스킬 애니메이션을 재생중 이라면 다른 애니메이션은 재생되지 않아야 하므로
        if (playerSkill.SkillCurrentlyUsing != null) return;
       
        playerState = state;
    }

    public void StopMovement()
    {
        stopMovement = true;
    }

    public void ContinueMovement()
    {
        stopMovement = false;
    }
    
    public float GetAnimationRunningTime(PlayerAnimations motion)
    {
        return playerAnimation.GetAnimationRunningTime(motion);
    }

    void SetPlayerForward(float dir)
    {
        if (dir != 0f)
        {
            var playerForward = dir > 0 ? RightYRotationValue : LeftYRotationValue;

            var playerXRotation = playerTransform.rotation.eulerAngles.x;
            var playerZRotation = playerTransform.rotation.eulerAngles.z;

            playerTransform.rotation = Quaternion.Euler(playerXRotation, playerForward, playerZRotation);
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
            case PlayerState.Attack: //TODO : 기본 공격 이외의 스킬 애니메이션들도 이곳에서 구분해 재생할 수 있도록 구현
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
}
