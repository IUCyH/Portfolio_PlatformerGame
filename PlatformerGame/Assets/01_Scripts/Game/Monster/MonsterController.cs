using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    const float RightYRotationValue = 0f;
    const float LeftYRotationValue = 180f;

    PlayerController playerCtr;
    MonsterAnimation monsterAnimation;
    Transform monsterTransform;

    [SerializeField]
    Vector3 maxMoveDistanceFromSpawnPos;
    [SerializeField]
    Vector3 maxRightPos;
    [SerializeField]
    Vector3 maxLeftPos;
    [SerializeField]
    float maxWallDetectionDist;
    [SerializeField]
    float maxPlayerDetectionDist;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float attackDamage;
    int playerLayer;
    int boundaryWallLayer;
    bool movingRightSide;
    bool isMoving;
    bool isAttacking;
    bool playerDetected;

    public void InitMonster(Transform parent)
    {
        monsterTransform = transform;
        playerCtr = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        monsterAnimation = new MonsterAnimation(GetComponent<Animator>());
        boundaryWallLayer = 1 << LayerMask.NameToLayer("BoundaryLayer");
        playerLayer = 1 << LayerMask.NameToLayer("Player");

        monsterTransform.SetParent(parent);
    }

    public void SetMonsterSpawnPos(Vector3 spawnPos)
    {
        monsterTransform.position = spawnPos;

        maxRightPos = spawnPos + maxMoveDistanceFromSpawnPos;
        maxLeftPos = spawnPos - maxMoveDistanceFromSpawnPos;
    }

    public void Move()
    {
        if (isAttacking)
        {
            isMoving = false;
            return;
        }

        if (movingRightSide)
        {
            MoveToRightSide();
        }
        else
        {
            MoveToLeftSide();
        }

        if (!isMoving)
        {
            monsterAnimation.Play(MonsterMotions.Move);
            isMoving = true;
        }
    }

    public void Attack()
    {
        var forward = GetMonsterForward();
        playerDetected = UseRayCast(forward, maxPlayerDetectionDist, playerLayer);

        if (!isAttacking && playerDetected) //애니메이션 재생 함수는 딱 한번만 호출되야 하므로 isAttacking이 false일때를 조건으로 추가
        {
            isAttacking = true;
            monsterAnimation.Play(MonsterMotions.Attack);
        }
        else if(!playerDetected) //isAttacking을 false로 만드는 조건은 플레이어가 감지되지 않았을때로 충분하므로 별다른 조건은 추가하지 않음
        {
            isAttacking = false;
        }
    }

    public void GiveDamageToPlayer()
    {
        if (!playerDetected) return;
        
        playerCtr.SetDamage(attackDamage);
    }

    void MoveToRightSide()
    {
        if (monsterTransform.position.x > maxRightPos.x || BoundaryWallDetected())
        {
            movingRightSide = false;
            SetMonsterForward(LeftYRotationValue);
            return;
        }

        monsterTransform.position += moveSpeed * Time.deltaTime * Vector3.right;
    }

    void MoveToLeftSide()
    {
        if (monsterTransform.position.x < maxLeftPos.x || BoundaryWallDetected())
        {
            movingRightSide = true;
            SetMonsterForward(RightYRotationValue);
            return;
        }

        monsterTransform.position += moveSpeed * Time.deltaTime * Vector3.left;
    }

    bool BoundaryWallDetected()
    {
        var forward = GetMonsterForward();
        var isDetected = UseRayCast(forward, maxWallDetectionDist, boundaryWallLayer);

        return isDetected;
    }

    Vector2 GetMonsterForward()
    {
        Vector2 forwardVector;
        
        if (movingRightSide)
        {
            forwardVector = Vector2.right;
        }
        else
        {
            forwardVector = Vector2.left;
        }

        return forwardVector;
    }

    RaycastHit2D UseRayCast(Vector2 direction, float distance, int layer)
    {
        Color rayColor = layer == playerLayer ? Color.magenta : Color.white;
        var debugPos = monsterTransform.position;
        debugPos.y += 0.5f;
        Debug.DrawRay(debugPos, direction * distance, rayColor);

        return Physics2D.Raycast(monsterTransform.position, direction, distance, layer);
    }

    void SetMonsterForward(float yRotationValue)
    {
        monsterTransform.rotation = Quaternion.Euler(0f, yRotationValue, 0f);
    }
}
