using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skills
{
    DefaultAttack,
    Dash,
    Max
}

public class PlayerSkill : MonoBehaviour
{
    [SerializeField]
    PlayerController playerCtr;
    
    IPlayerSkill[] skills = new IPlayerSkill[(int)Skills.Max];

    int indexOfExecutingSkill;
    [SerializeField]
    float currSkillGauge;
    
    bool executingSkill;

    public void ExecuteSkills()
    {
        if (executingSkill) return;

        for (int i = 0; i < skills.Length; i++)
        {
            var key = (Key)((int)Key.DefaultAttack + i);

            bool canUse = !skills[i].NotReadyForExecute && (currSkillGauge >= skills[i].GaugeUsage);
            if (InputManager.GetKeyDown(key) && canUse)
            {
                skills[i].Execute();

                if (key != Key.DefaultAttack)
                {
                    indexOfExecutingSkill = i;
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

    void Start()
    {
        skills = GetComponentsInChildren<IPlayerSkill>();
        currSkillGauge = 100f; //임시로 게이지의 기본 최대치 설정
        for (int i = 0; i < skills.Length; i++) Debug.Log(skills[i]);
    }

    void Update()
    {
        executingSkill = skills[indexOfExecutingSkill].ExecutingSkill;
    }
}
