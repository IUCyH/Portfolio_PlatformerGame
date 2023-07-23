using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    Transform monsterTransform;
    
    [SerializeField]
    Vector3 spawnPos;
    [SerializeField]
    Vector3 maxRightPos;
    [SerializeField]
    Vector3 maxLeftPos;
    [SerializeField]
    float moveSpeed;
    bool movingRightSide;
    
    public void Init(Transform parent)
    {
        monsterTransform = transform;
     
        monsterTransform.SetParent(parent);
        monsterTransform.position = spawnPos;
        gameObject.SetActive(true);
    }

    void Move()
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
            return;
        }

        monsterTransform.position += moveSpeed * Time.deltaTime * Vector3.right;
    }

    void MoveToLeftSide()
    {
        if (monsterTransform.position.x < maxLeftPos.x)
        {
            movingRightSide = true;
            return;
        }

        monsterTransform.position += moveSpeed * Time.deltaTime * Vector3.left;
    }
}
