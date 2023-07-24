using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : Singleton<PlayerUIManager>
{
    StringBuilder sb = new StringBuilder();
    [SerializeField]
    TextMeshProUGUI[] skillCooldownTexts;
    [SerializeField]
    TextMeshProUGUI jumpCountText;
    [SerializeField]
    Image skillGaugeImg;
    [SerializeField]
    Image hpBarImg;
    
    //Prev Values
    int prevJumpCount;

    public void UpdateSkillGauge(float value, bool isDecrease = true)
    {
        if (isDecrease)
        {
            skillGaugeImg.fillAmount -= value;
        }
        else
        {
            skillGaugeImg.fillAmount += value;
        }
    }

    public void UpdateHpBar(float value, bool isDecrease = true)
    {
        if (isDecrease)
        {
            hpBarImg.fillAmount -= value;
        }
        else
        {
            hpBarImg.fillAmount += value;
        }
    }

    public void UpdateSkillCooldownText(Skills skill, float cooldown)
    {
        float result = (float)Math.Round(cooldown, 2);
        
        sb.Clear();
        sb.Append(result);
        skillCooldownTexts[(int)skill].text = sb.ToString();
    }

    public void UpdateJumpCountText(int count)
    {
        if (prevJumpCount == count) return; //GC 최적화 위해 이전값과 비교했을때 변동사항이 없다면 text를 업데이트 하지 않음
        
        sb.Clear();
        sb.Append(count);
        jumpCountText.text = sb.ToString();
        
        prevJumpCount = count;
    }
}
