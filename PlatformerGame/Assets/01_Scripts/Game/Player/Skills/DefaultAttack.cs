using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAttack : MonoBehaviour, IPlayerSkill
{
    [SerializeField]
    PlayerController playerCtr;
    
    public bool NotReadyForExecute { get; set; }
    public bool ExecutingSkill { get; set; }
    public float GaugeUsage { get; set; }

    public void Execute()
    {
        playerCtr.SetPlayerState(PlayerState.Attack);
    }

    public void CalculateCooldown()
    {
        
    }
}
