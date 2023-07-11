using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour, IPlayerSkill
{
    [SerializeField]
    PlayerSkill playerSkill;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    Transform playerTransform;
    
    [SerializeField]
    Vector3 dashDistance;
    [SerializeField]
    Vector3 targetVector;
    
    float cooldownTimer;
    [SerializeField]
    float maxCooldown;
    
    [SerializeField]
    float dashSpeed;

    public bool NotReadyForExecute { get; set; }
    public float GaugeUsage { get; set; }

    IEnumerator Coroutine_Update()
    {
        while (true)
        {
            if (targetVector != Vector3.zero)
            {
                var dir = Vector3.right * playerTransform.localScale.x;
                playerTransform.position += dashSpeed * Time.deltaTime * dir;
                
                bool isOverThanTarget = dir.x > 0f ? playerTransform.position.x > targetVector.x : playerTransform.position.x < targetVector.x;

                if (isOverThanTarget)
                {
                    Debug.Log("Stop");
                    playerController.ContinueMovement();
                    
                    playerSkill.ExecutingSkill = false;
                    targetVector = Vector3.zero;
                }
            }

            CalculateCooldown();

            yield return null;
        }
    }

    public void Execute()
    {
        //Debug.Log("Execute Dash");
        playerController.StopMovement();
        
        cooldownTimer = maxCooldown;
        playerSkill.ExecutingSkill = true;
        NotReadyForExecute = true;

        targetVector = playerTransform.position + playerTransform.localScale.x * dashDistance;
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
        playerSkill.UpdateCooldownUIText(Skills.DefaultAttack, cooldownTimer);
    }

    void Start()
    {
        var playerTrans = transform.parent.parent;
        
        playerTransform = playerTrans;
        playerSkill = playerTrans.GetComponent<PlayerSkill>();
        playerController = playerTransform.GetComponent<PlayerController>();

        GaugeUsage = 1f;
        cooldownTimer = maxCooldown;
        
        StartCoroutine(Coroutine_Update());
    }
}
