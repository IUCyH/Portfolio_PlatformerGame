using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : Singleton<PlayerUIManager>
{
    StringBuilder sb = new StringBuilder();

    public void UpdateFillAmountOfSkillGaugeImage(Image skillGauge, float value, bool isDecrease = true)
    {
        if (isDecrease)
        {
            skillGauge.fillAmount -= value;
        }
        else
        {
            skillGauge.fillAmount += value;
        }
    }

    public void UpdateFillAmountOfHpBarImage(Image hpBar, float value, bool isDecrease = true)
    {
        if (isDecrease)
        {
            hpBar.fillAmount -= value;
        }
        else
        {
            hpBar.fillAmount += value;
        }
    }

    public void UpdateSkillCooldownText(TextMeshProUGUI cooldownText, float cooldown)
    {
        sb.Clear();
        sb.Append(cooldown);
        cooldownText.text = sb.ToString();
    }

    public void UpdateJumpCountText(TextMeshProUGUI jumpCountText, int count)
    {
        sb.Clear();
        sb.Append(count);
        jumpCountText.text = sb.ToString();
    }
}
