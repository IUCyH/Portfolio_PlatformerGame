using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum StateBar
{
    levelUpCost,
}

public class GameSystemManager : Singleton<GameSystemManager>
{
    const float SaveTiming = 0.5f;
    
    [SerializeField]
    Image levelCostBar;
    [SerializeField]
    TextMeshProUGUI levelText;

    float saveTimer;

    protected override void OnStart()
    {
        levelCostBar.fillAmount = DataManager.Instance.PlayerData.levelUpProgress;
        levelText.text = DataManager.Instance.PlayerData.level.ToString();
    }

    void Update()
    {
        saveTimer += Time.deltaTime;
        if (saveTimer > SaveTiming)
        {
            DataManager.Instance.Save();
            saveTimer = 0f;
        }
    }

    public void UpdateStateBar(StateBar state, float cost)
    {
        DataManager.Instance.PlayerData.levelUpProgress += cost;
        InGameUIManager.Instance.UpdateImageFillAmount(levelCostBar, cost, false);
        if (levelCostBar.fillAmount >= 1f)
        {
            var level = ++DataManager.Instance.PlayerData.level;
            levelText.text = level.ToString();
            InGameUIManager.Instance.SetImageFillAmount(levelCostBar, 0f);
            DataManager.Instance.PlayerData.levelUpProgress = 0f;
        }
    }
}
