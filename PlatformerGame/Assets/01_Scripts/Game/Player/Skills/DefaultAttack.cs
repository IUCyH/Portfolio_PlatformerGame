using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAttack : MonoBehaviour, IPlayerSkill
{
    public bool NotReadyForExecute { get; set; }
    public bool ExecutingSkill { get; set; }
    public float GaugeUsage { get; set; }

    public void Execute()
    {
        
    }

    public void CalculateCooldown()
    {
        
    }
}
