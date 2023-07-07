using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    enum Skills
    {
        DefaultAttack,
        Dash,
        Max
    }

    IPlayerSkill[] skills = new IPlayerSkill[(int)Skills.Max];

    bool executingSkill;

    public bool ExecutingSkill { get => executingSkill; set => executingSkill = value; }
    
    public void ExecuteSkills()
    {
        Debug.Log(executingSkill);
        if (executingSkill) return;
        
        for (int i = 0; i < skills.Length; i++)
        {
            var key = (Key)((int)Key.DefaultAttack + i);
            Debug.Log(key);
            if(InputManager.GetKeyDown(key) && !skills[i].NotReadyForExecute)
            {
                skills[i].Execute();
                return;
            }
        }
    }

    void Start()
    {
        skills = GetComponentsInChildren<IPlayerSkill>();
        for (int i = 0; i < skills.Length; i++) Debug.Log(skills[i]);
    }
}
