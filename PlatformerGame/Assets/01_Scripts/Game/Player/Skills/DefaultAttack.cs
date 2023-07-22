using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAttack : MonoBehaviour, IPlayerSkill
{
    [SerializeField]
    PlayerController playerCtr;

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

                if (durationTimer >= duration)
                {
                    durationTimer = 0f;
                    
                    SkillIsRunning = false;
                    NotReadyForExecute = false;
                    
                    playerCtr.SetPlayerState(PlayerState.Idle);
                }
                Debug.Log("Default attack is running?" + SkillIsRunning);
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
        
    }
}
