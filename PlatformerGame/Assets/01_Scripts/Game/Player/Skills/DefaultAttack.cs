using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAttack : MonoBehaviour, IPlayerSkill
{
    [SerializeField]
    PlayerController playerCtr;
    
    public bool NotReadyForExecute { get; set; }
    public bool SkillRunning { get; set; }
    public float GaugeUsage { get; set; }

    public void Execute()
    {
        Debug.Log("Is True : " + NotReadyForExecute);
        playerCtr.SetPlayerState(PlayerState.Attack);
        
        SkillRunning = true;
        NotReadyForExecute = true;
        
        Invoke(nameof(AB), 0.5f);
    }

    void AB()
    {
        playerCtr.SetPlayerState(PlayerState.Idle);
        
        SkillRunning = false;
        NotReadyForExecute = false;
        Debug.Log("Done");
    }

    public void CalculateCooldown()
    {
        
    }
}
