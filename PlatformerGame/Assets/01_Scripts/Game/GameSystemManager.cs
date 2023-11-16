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
    [SerializeField]
    Image levelCostBar;
    [SerializeField]
    TextMeshProUGUI levelText;

    int level = 1;

    protected override void OnStart()
    {
        levelCostBar.fillAmount = 0f;
        InitGameSprites();
    }

    void InitGameSprites()
    {
        var sprites = FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].CompareTag("Player") || sprites[i].CompareTag("Monster")) continue;

            sprites[i].sprite = SpriteTable.Instance.GetSprite(KindOfAssetBundle.UI, sprites[i].tag);
        }
    }

    public void UpdateStateBar(StateBar state, float cost)
    {
        InGameUIManager.Instance.UpdateImageFillAmount(levelCostBar, 1 / cost, false);
        if (levelCostBar.fillAmount >= 1f)
        {
            level++;
            levelText.text = level.ToString();
            InGameUIManager.Instance.SetImageFillAmount(levelCostBar, 0f);
        }
    }
}
