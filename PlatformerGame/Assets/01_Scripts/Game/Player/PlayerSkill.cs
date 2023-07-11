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

    [SerializeField]
    PlayerController playerCtr;
    IPlayerSkill[] skills = new IPlayerSkill[(int)Skills.Max];

    [SerializeField]
    float currSkillGauge;
    
    bool executingSkill;

    public bool ExecutingSkill { get => executingSkill; set => executingSkill = value; }
    
    public void ExecuteSkills()
    {
        //Debug.Log(executingSkill);
        if (executingSkill) return;
        
        for (int i = 0; i < skills.Length; i++)
        {
            var key = (Key)((int)Key.DefaultAttack + i);
            //Debug.Log(key);
            bool canUse = !skills[i].NotReadyForExecute && (currSkillGauge >= skills[i].GaugeUsage);
            if(InputManager.GetKeyDown(key) && canUse)
            {
                skills[i].Execute();
                DecreaseGauge(skills[i].GaugeUsage);
                return;
            }
        }
    }

    void DecreaseGauge(float gaugeWillUse)
    {
        var gaugeUsage = gaugeWillUse / 100f; //최대치를 100으로 가정하고 이미지의 fill amount는 0~1로 정규화 되있으므로 최대치로 나눠 정규화 시킨다.

        currSkillGauge -= gaugeWillUse; //currSkillGauge는 정규화 되지 않은 값이므로 그냥 쓸 게이지 양만큼을 빼준다.
        playerCtr.UpdateSkillGauge(gaugeUsage);
    }

    void Start()
    {
        skills = GetComponentsInChildren<IPlayerSkill>();
        currSkillGauge = 100f;
        for (int i = 0; i < skills.Length; i++) Debug.Log(skills[i]);
    }
}
