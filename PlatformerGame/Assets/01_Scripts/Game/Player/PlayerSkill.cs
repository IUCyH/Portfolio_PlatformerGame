using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skills
{
    None = -1,
    DefaultAttack,
    Dash,
    Max
}

public class PlayerSkill : MonoBehaviour
{
    [SerializeField]
    PlayerController playerCtr;
    
    IPlayerSkill[] skills = new IPlayerSkill[(int)Skills.Max];

    int indexOfRunningSkill;
    [SerializeField]
    float currSkillGauge;
    bool skillInUse;
    
    public IPlayerSkill SkillCurrentlyUsing { get; private set; }
    
    void Start()
    {
        skills = GetComponentsInChildren<IPlayerSkill>();
        currSkillGauge = 100f; //임시로 게이지의 기본 최대치 설정
        for (int i = 0; i < skills.Length; i++) Debug.Log(skills[i]);
    }

    void Update()
    {
        if (indexOfRunningSkill != (int)Skills.None)
        {
            var skill = skills[indexOfRunningSkill];
            var isRunning = skill.SkillIsRunning;

            if (isRunning && indexOfRunningSkill > (int)Skills.DefaultAttack)
            {
                skillInUse = true;
            }

            else if(!isRunning)
            {
                SkillCurrentlyUsing = null;
                indexOfRunningSkill = (int)Skills.None;

                skillInUse = false;
            }
        }
        //Debug.Log(skills[0].SkillIsRunning);
    }

    public void ExecuteSkills()
    {
        if (skillInUse) return;

        for (int i = 0; i < skills.Length; i++)
        {
            var key = (Key)((int)Key.DefaultAttack + i);

            bool canUse = !skills[i].NotReadyForExecute && (currSkillGauge >= skills[i].GaugeUsage);
            if (InputManager.GetKeyDown(key) && canUse)
            {
                skills[i].Execute();
                SkillCurrentlyUsing = skills[i];
                indexOfRunningSkill = i;
                
                if (key != Key.DefaultAttack)
                {
                    DecreaseGauge(skills[i].GaugeUsage);

                    return;
                }
            }
        }
    }

    void DecreaseGauge(float gaugeWillUse)
    {
        var gaugeUsage = gaugeWillUse / 100f; //최대치를 100으로 가정하고 이미지의 fill amount는 0~1로 정규화 되있으므로 최대치로 나눠 정규화 시킨다.

        currSkillGauge -= gaugeWillUse; //currSkillGauge는 정규화 되지 않은 값이므로 그냥 쓸 게이지 양만큼을 빼준다.
        playerCtr.UpdateSkillGauge(gaugeUsage);
    }
}
