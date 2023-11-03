using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : Singleton<InGameUIManager>
{
    [SerializeField]
    InventoryController inventory;
    StringBuilder sb = new StringBuilder();

    protected override void OnStart()
    {
        inventory.gameObject.SetActive(false);
    }

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
    
    public void SetImageFillAmount(Image img, float value)
    {
        img.fillAmount = value;
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

    public void OpenOrHideInventory()
    {
        var isOpen = !inventory.gameObject.activeSelf;
        inventory.gameObject.SetActive(isOpen);
    }
}










































