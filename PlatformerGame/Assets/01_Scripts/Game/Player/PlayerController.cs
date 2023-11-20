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

    MovingPlatformController movingPlatform;
    PlayerAnimation playerAnimation;
    [SerializeField]
    PlayerMove playerMove;
    [SerializeField]
    PlayerJump playerJump;
    [SerializeField]
    PlayerSkill playerSkill;
    Transform playerTransform;
    [SerializeField]
    Image hpBar;
    
    PlayerState playerState = PlayerState.Idle;
    bool stopMovement;

    float MaxHP => DataManager.Instance.PlayerData.maxHP;
    float Hp { get => DataManager.Instance.PlayerData.hp; set => DataManager.Instance.PlayerData.hp = value; }

    void Start()
    {
        if(Hp <= 0f) Hp = 100f;
        hpBar.fillAmount = Hp / MaxHP;
        playerTransform = transform;
        playerAnimation = new PlayerAnimation(GetComponent<Animator>());
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
            playerJump.CheckCanJump();
        }
        playerJump.SetJumpCountToZeroWhenPlayerOnTheGround();

        playerSkill.ExecuteSkills();
        
        PlayAnimationByPlayerState();
        
        if (!ReferenceEquals(movingPlatform, null) && !playerMove.IsMoving && !playerJump.IsJumping)
        {
            playerTransform.position += movingPlatform.MoveDistancePerFrame;
        }

        if (InputManager.GetKeyDown(Key.OpenInventory))
        {
            InGameUIManager.Instance.OpenOrHideInventory();
        }
    }

    void FixedUpdate()
    {
        playerJump.Jump();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("MovingPlatform")) return;

        movingPlatform = other.gameObject.GetComponent<MovingPlatformController>();
        movingPlatform.OnPlayerGoUp();
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (!ReferenceEquals(movingPlatform, null))
        {
            movingPlatform.OnPlayerGoDown();
            movingPlatform = null;
        }
    }

    public void SetDamage(float damage)
    {
        if (Hp <= 0f) return;
        
        Hp -= damage;
        InGameUIManager.Instance.UpdateImageFillAmount(hpBar, damage / MaxHP);
    }
    
    public void SetPlayerState(PlayerState state)
    {
        //플레이어가 스킬 애니메이션을 재생중 이라면 다른 애니메이션은 재생되지 않아야 하므로
        if (playerSkill.SkillInUse) return;
       
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
        return playerAnimation.GetRunningTime(motion);
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
        playerAnimation.Play(motion);
    }

    void PlayAnimationByPlayerState()
    {
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
    }
}
