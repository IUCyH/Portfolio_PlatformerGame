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
    Transform playerTransform;

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
    float timeSinceChasingPlayer;
    [SerializeField]
    float maxTimeForChasingPlayer;
    int playerLayer;
    int boundaryWallLayer;
    bool isChasing;
    bool movingRightSide;
    bool moveAnimIsPlaying;

    public void InitMonster(Transform parent)
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
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
        if (isChasing) return;

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

    public void ChasePlayer()
    {
        var playerDetected = DetectPlayer();

        if (!isChasing && playerDetected)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            if (!playerDetected && timeSinceChasingPlayer <= maxTimeForChasingPlayer)
            {
                timeSinceChasingPlayer += Time.deltaTime;
            }
            else if (timeSinceChasingPlayer > maxTimeForChasingPlayer)
            {
                timeSinceChasingPlayer = 0f;
                isChasing = false;
            }
            else
            {
                timeSinceChasingPlayer = 0f;
            }

            //TODO : Make Chase Logic
        }
    }

    bool DetectPlayer()
    {
        Vector3 distBetweenThisAndPlayer = (playerTransform.position - monsterTransform.position).normalized;
        var playerDetected = UseRayCast(distBetweenThisAndPlayer, maxPlayerDetectionDist, playerLayer);

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

    bool UseRayCast(Vector2 direction, float distance, int layer)
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
