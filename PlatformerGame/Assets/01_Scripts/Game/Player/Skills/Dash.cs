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
    float cooldownTimer;
    [SerializeField]
    float maxCooldown;
    [SerializeField]
    float dashSpeed;
    [SerializeField]
    float attackDamage;
    bool collisionWithMonster;

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
                bool shouldStopDash = collisionWithMonster || overTheTarget;

                if (shouldStopDash)
                {
                    GiveDamageToMonsters();
                    
                    SkillIsRunning = false;
                    collisionWithMonster = false;
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
        
        StartCoroutine(Coroutine_Update());
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(!SkillIsRunning) return;
        
        if (other.gameObject.CompareTag(MonsterTag))
        {
            collisionWithMonster = true;
        } //RayCast로 변경 예정
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
