using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager>
{
    StringBuilder sb = new StringBuilder();

    public void UpdateImageFillAmount(Image img, float value, bool isDecrease = true)
    {
        if (isDecrease)
        {
            img.fillAmount -= value;
        }
        else
        {
            img.fillAmount += value;
        }
    }

    public void UpdateText(TextMeshProUGUI text, float value)
    {
        sb.Clear();
        sb.Append(value);
        text.text = sb.ToString();
    }

    public void UpdateText(TextMeshProUGUI text, int value)
    {
        sb.Clear();
        sb.Append(value);
        text.text = sb.ToString();
    }
}










































