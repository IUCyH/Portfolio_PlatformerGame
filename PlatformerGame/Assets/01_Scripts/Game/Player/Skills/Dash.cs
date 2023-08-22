using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dash : MonoBehaviour, IPlayerSkill
{
    const string MonsterTag = "Monster";
    const float LeftRotation = 180f;

    [SerializeField]
    MultipleAttackArea attackArea;
    [SerializeField]
    PlayerController playerCtr;
    [SerializeField]
    PlayerSkill playerSkill;
    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    Vector3 dashDistance;
    [SerializeField]
    Vector3 targetVector;
    Vector3 dashDir;
    int monsterLayer;
    float cooldownTimer;
    [SerializeField]
    float maxCooldown;
    [SerializeField]
    float dashSpeed;
    [SerializeField]
    float attackDamage;
    [SerializeField]
    float monsterDetectionDistance;

    public bool NotReadyForExecute { get; set; }
    public bool SkillIsRunning { get; set; }
    public float GaugeUsage { get; set; }

    IEnumerator Coroutine_Update()
    {
        while (true)
        {
            if (SkillIsRunning)
            {
                playerTransform.position += dashSpeed * Time.deltaTime * dashDir;

                bool overTheTarget = dashDir.x > 0f ? playerTransform.position.x > targetVector.x : playerTransform.position.x < targetVector.x;
                bool shouldStopDash = CollidedWithMonster() || overTheTarget;

                if (shouldStopDash)
                {
                    GiveDamageToMonsters();
                    
                    SkillIsRunning = false;
                    playerCtr.ContinueMovement();
                }
            }

            CalculateCooldown();

            yield return null;
        }
    }
    
    void Start()
    {
        GaugeUsage = 1f;
        cooldownTimer = maxCooldown;
        monsterLayer = 1 << LayerMask.NameToLayer("Monster");
        
        StartCoroutine(Coroutine_Update());
    }

    public void Execute()
    {
        var playerForward = GetPlayerForward();
        playerCtr.StopMovement();
        
        cooldownTimer = maxCooldown;
        NotReadyForExecute = true;
        SkillIsRunning = true;

        targetVector = playerTransform.position + playerForward * dashDistance;
        dashDir = Vector3.right * playerForward;
    }

    public void CalculateCooldown()
    {
        if (!NotReadyForExecute) return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            cooldownTimer = 0f;
            NotReadyForExecute = false;
        }
        playerSkill.UpdateCooldownText(Skills.Dash, cooldownTimer);
    }

    public void GiveDamageToMonsters()
    {
        if (!SkillIsRunning) return;
        
        var monsters = attackArea.MonstersInsideOfArea;

        for (int i = 0; i < monsters.Count; i++)
        {
            var monster = monsters[i].GetComponent<MonsterController>();
            if (!ReferenceEquals(monster, null))
            {
                monster.SetDamage(attackDamage);
            }
        }
    }

    bool CollidedWithMonster()
    {
        var playerForward = new Vector2(GetPlayerForward(), 0f);
        var isCollided = Physics2D.Raycast(playerTransform.position, playerForward, monsterDetectionDistance, monsterLayer);
        Debug.DrawRay(playerTransform.position, playerForward * monsterDetectionDistance, Color.red);
        return isCollided;
    }

    float GetPlayerForward()
    {
        var yRotation = playerTransform.rotation.eulerAngles.y;
        if (Mathf.Approximately(yRotation, LeftRotation))
        {
            return -1f;
        }

        return 1f;
    }
}
