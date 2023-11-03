using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupType
{
    Ok,
    OkCancel,
    InputField
}

public class PopupManager : Singleton_DontDestroy<PopupManager>
{
    List<ObjectPool<IPopup>> popupPools;
    [SerializeField]
    RectTransform popupCanvasRectTransform;

    protected override void OnStart()
    {
        var popups = Resources.LoadAll<GameObject>("Prefabs");
        popupPools = new List<ObjectPool<IPopup>>();

        for (int i = 0; i < popups.Length; i++)
        {
            var popup = popups[i];
            popupPools.Add(new ObjectPool<IPopup>(3, () =>
            {
                var obj = Instantiate(popup);
                var rectTransform = obj.GetComponent<RectTransform>();

                rectTransform.SetParent(popupCanvasRectTransform);
                rectTransform.localPosition = Vector3.zero;
                rectTransform.localScale = Vector3.one;
                
                obj.SetActive(false);
                return obj.GetComponent<IPopup>();
            }));
        }
    }

    public void OpenPopup(PopupType type, string title, string content)
    {
        var popup = popupPools[(int)type].Get();
        popup.SetPopup(title, content);
        
        WindowManager.Instance.OpenAndPushIntoStack((IWindow)popup);
    }

    public void ClosePopup(PopupType type)
    {
        var popupObj = WindowManager.Instance.CloseAndPopFromStack();
        if (popupObj == null) return;
        
        var popup = (IPopup)popupObj;
        if (popup.Type == type)
        {
            popupPools[(int)type].Set(popup);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenPopup(PopupType.Ok, "TEST", "IT'S JUST A TEST");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenPopup(PopupType.OkCancel, "TEST", "IT'S JUST A TEST");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenPopup(PopupType.InputField, "TEST", "IT'S JUST A TEST");
        }
    }
}
