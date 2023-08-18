using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAttack : MonoBehaviour, IPlayerSkill
{
    [SerializeField]
    PlayerController playerCtr;
    [SerializeField]
    MultipleAttackArea attackArea;

    [SerializeField]
    float attackDamage;
    float durationTimer;
    float duration;
    
    public bool NotReadyForExecute { get; set; }
    public bool SkillIsRunning { get; set; }
    public float GaugeUsage { get; set; }

    IEnumerator Coroutine_Update()
    {
        while (true)
        {
            if (SkillIsRunning)
            {
                durationTimer += Time.deltaTime;

                if (durationTimer > duration)
                {
                    GiveDamageToMonsters();
                    durationTimer = 0f;
                    
                    SkillIsRunning = false;
                    NotReadyForExecute = false;
                    
                    playerCtr.SetPlayerState(PlayerState.Idle);
                }
            }
            
            yield return null;
        }
    }
    
    void Start()
    {
        duration = playerCtr.GetAnimationRunningTime(PlayerAnimations.Attack);
        StartCoroutine(Coroutine_Update());
    }

    public void Execute()
    {
        playerCtr.SetPlayerState(PlayerState.Attack);
        
        SkillIsRunning = true;
        NotReadyForExecute = true;
    }

    public void CalculateCooldown()
    {
        //기본공격은 쿨타임을 갖고있지 않지만 인터페이스 상속으로 인해 함수를 지울수는 없으니 비워놓는다
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
}
