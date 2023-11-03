using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    const float RightYRotationValue = 0f;
    const float LeftYRotationValue = 180f;

    StringBuilder sb = new StringBuilder();
    PlayerController playerCtr;
    MonsterAnimation monsterAnimation;
    Transform monsterTransform;
    SpriteRenderer monsterSprite;

    Vector3 dir = Vector3.right;
    Vector3 originPos;
    float targetPosX;
    [SerializeField]
    float maxDistance;
    [SerializeField]
    float maxWallDetectionDist;
    [SerializeField]
    float maxPlayerDetectionDist;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float attackDamage;
    [SerializeField]
    float maxHp;
    float hp;
    int playerLayer;
    int boundaryWallLayer;
    bool isMoving;
    bool isAttacking;
    bool playerDetected;
    
    public float LevelUpCost { get; private set; }

    public void InitMonster(Transform parent)
    {
        monsterTransform = transform;
        hp = maxHp;
        playerCtr = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        monsterAnimation = new MonsterAnimation(GetComponent<Animator>());
        boundaryWallLayer = 1 << LayerMask.NameToLayer("BoundaryLayer");
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        monsterSprite = GetComponent<SpriteRenderer>();

        monsterTransform.SetParent(parent);
        originPos = monsterTransform.position;
        targetPosX = originPos.x + maxDistance * dir.x;
        LevelUpCost = 3f;
    }

    public void SetMonsterSpawnPos(Vector3 spawnPos)
    {
        monsterTransform.position = spawnPos;
    }

    public void Move()
    {
        if (isAttacking)
        {
            isMoving = false;
            return;
        }
        
        if ((targetPosX - monsterTransform.position.x) * dir.x <= 0f || BoundaryWallDetected())
        {
            dir *= -1;
            targetPosX = originPos.x + maxDistance * dir.x;
            
            SetMonsterForward(dir.x);
        }
        
        monsterTransform.position += moveSpeed * Time.deltaTime * dir;

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

    public void SetDamage(float damage)
    {
        var pos = transform.position;
        pos.x += monsterSprite.size.x / 2;
        pos.y += (monsterSprite.size.y / 2);
        
        hp -= damage;
        sb.Clear();
        sb.Append(damage);
        DamageHUDManager.Instance.ShowDamageHUD(pos, sb.ToString());
        
        if (hp < 0f)
        {
            hp = maxHp;
            GameSystemManager.Instance.UpdateStateBar(StateBar.levelUpCost, LevelUpCost);
            gameObject.SetActive(false);
            MonsterManager.Instance.DestroyMonster(this);
        }
    }

    public void GiveDamageToPlayer()
    {
        if (!playerDetected) return;
        
        playerCtr.SetDamage(attackDamage);
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
        
        if (dir.x > 0)
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

    void SetMonsterForward(float dirX)
    {
        var rotation = dirX > 0 ? RightYRotationValue : LeftYRotationValue;

        monsterTransform.rotation = Quaternion.Euler(0f, rotation, 0f);
    }
}
