using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    const float RightYRotationValue = 0f;
    const float LeftYRotationValue = 180f;
    
    Transform monsterTransform;

    [SerializeField]
    Vector3 maxMoveDistanceFromSpawnPos;
    [SerializeField]
    Vector3 maxRightPos;
    [SerializeField]
    Vector3 maxLeftPos;
    [SerializeField]
    float maxRaycastDistance;
    [SerializeField]
    float moveSpeed;
    int boundaryWallLayerMask;
    bool movingRightSide;
    
    public void InitMonster(Transform parent)
    {
        monsterTransform = transform;
        boundaryWallLayerMask = 1 << LayerMask.NameToLayer("BoundaryLayer");
     
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
        var isDetected = Physics2D.Raycast(monsterTransform.position, forward, maxRaycastDistance, boundaryWallLayerMask);

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

    void SetMonsterForward(float yRotationValue)
    {
        monsterTransform.rotation = Quaternion.Euler(0f, yRotationValue, 0f);
    }
}
