using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    const float RightYRotationValue = 0f;
    const float LeftYRotationValue = 180f;

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
    int playerLayer;
    int boundaryWallLayer;
    bool movingRightSide;
    bool moveAnimIsPlaying;
    
    public void InitMonster(Transform parent)
    {
        monsterTransform = transform;
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
        if (movingRightSide)
        {
            MoveToRightSide();
        }
        else
        {
            MoveToLeftSide();
        }

        if (!moveAnimIsPlaying)
        {
            monsterAnimation.Play(MonsterMotions.Move);
            moveAnimIsPlaying = true;
        }
    }

    public void AttackWhenDetectedPlayer()
    {
        if (CanAttack())
        {
            Debug.Log("Attack Player!");
        }
    }

    bool CanAttack()
    {
        var playerDetected = UseRayCast(maxPlayerDetectionDist, playerLayer);

        return playerDetected;
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
        var isDetected = UseRayCast(maxWallDetectionDist, boundaryWallLayer);

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

    bool UseRayCast(float distance, int layer)
    {
        var forward = GetMonsterForward();

        Color rayColor = layer == playerLayer ? Color.magenta : Color.white;
        var debugPos = monsterTransform.position;
        debugPos.y += 0.5f;
        Debug.DrawRay(debugPos, forward * distance, rayColor);
        
        return Physics2D.Raycast(monsterTransform.position, forward, distance, layer);
    }

    void SetMonsterForward(float yRotationValue)
    {
        monsterTransform.rotation = Quaternion.Euler(0f, yRotationValue, 0f);
    }
}
