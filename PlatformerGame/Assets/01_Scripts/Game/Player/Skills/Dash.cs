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
    float cooldown;
    
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
                    cooldownTimer = 0f;
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
        
        playerSkill.ExecutingSkill = true;
        NotReadyForExecute = true;

        targetVector = playerTransform.position + playerTransform.localScale.x * dashDistance;
    }

    public void CalculateCooldown()
    {
        if(playerSkill.ExecutingSkill) return;
        
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer > cooldown)
        {
            cooldownTimer = 0f;
            NotReadyForExecute = false;
        }
    }

    void Start()
    {
        var playerTrans = transform.parent.parent;
        
        playerTransform = playerTrans;
        playerSkill = playerTrans.GetComponent<PlayerSkill>();
        playerController = playerTransform.GetComponent<PlayerController>();

        GaugeUsage = 1f;

        StartCoroutine(Coroutine_Update());
    }
}
