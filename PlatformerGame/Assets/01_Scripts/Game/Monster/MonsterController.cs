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
    Vector3 additionalPosFromSpawnPos;
    Vector3 maxRightPos;
    Vector3 maxLeftPos;
    [SerializeField]
    float moveSpeed;
    bool movingRightSide;
    
    public void InitMonster(Transform parent)
    {
        monsterTransform = transform;
     
        monsterTransform.SetParent(parent);
    }

    public void SetMonsterSpawnPos(Vector3 spawnPos)
    {
        monsterTransform.position = spawnPos;
        
        maxRightPos = additionalPosFromSpawnPos + spawnPos;
        maxLeftPos = additionalPosFromSpawnPos - spawnPos;
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
        if (monsterTransform.position.x > maxRightPos.x)
        {
            movingRightSide = false;
            SetMonsterForward(LeftYRotationValue);
            return;
        }

        monsterTransform.position += moveSpeed * Time.deltaTime * Vector3.right;
    }

    void MoveToLeftSide()
    {
        if (monsterTransform.position.x < maxLeftPos.x)
        {
            movingRightSide = true;
            SetMonsterForward(RightYRotationValue);
            return;
        }

        monsterTransform.position += moveSpeed * Time.deltaTime * Vector3.left;
    }

    void SetMonsterForward(float yRotationValue)
    {
        monsterTransform.rotation = Quaternion.Euler(0f, yRotationValue, 0f);
    }
}
