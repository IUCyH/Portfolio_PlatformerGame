using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerSkill
{
    bool NotReadyForExecute { get; set; }
    bool SkillIsRunning { get; set; }
    float GaugeUsage { get; set; }

    void Execute();
    void CalculateCooldown();
    void GiveDamageToMonsters();
}
