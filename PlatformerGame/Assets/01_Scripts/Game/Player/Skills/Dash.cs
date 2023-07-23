using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour, IPlayerSkill
{
    const float LeftRotation = 180f;
    
    [SerializeField]
    PlayerController playerCtr;
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
                
                bool isOverThanTarget = dashDir.x > 0f ? playerTransform.position.x > targetVector.x : playerTransform.position.x < targetVector.x;

                if (isOverThanTarget)
                {
                    Debug.Log("Stop");
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
        var playerTrans = transform.parent.parent;
        
        playerTransform = playerTrans;
        playerCtr = playerTransform.GetComponent<PlayerController>();

        GaugeUsage = 1f;
        cooldownTimer = maxCooldown;
        
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
        playerCtr.UpdateSkillCooldownText(Skills.Dash, cooldownTimer);
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
