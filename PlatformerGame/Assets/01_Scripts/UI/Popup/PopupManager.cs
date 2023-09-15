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
    Dictionary<PopupType, ObjectPool<IPopup>> popupPools;
    [SerializeField]
    RectTransform popupCanvasRectTransform;

    protected override void OnStart()
    {
        var popups = Resources.LoadAll<GameObject>("Prefabs");
        popupPools = new Dictionary<PopupType, ObjectPool<IPopup>>();

        for (int i = 0; i < popups.Length; i++)
        {
            var popup = popups[i];
            var pool = new ObjectPool<IPopup>(3, () =>
            {
                var obj = Instantiate(popup);
                var rectTransform = obj.GetComponent<RectTransform>();

                rectTransform.SetParent(popupCanvasRectTransform);
                rectTransform.localPosition = Vector3.zero;
                rectTransform.localScale = Vector3.one;
                
                obj.SetActive(false);
                return obj.GetComponent<IPopup>();
            });
            
            popupPools.Add((PopupType)i, pool);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            var popup = popupPools[PopupType.Ok].Get();
            WindowManager.Instance.OpenWindow(popup.ThisGameObject);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            var popup = WindowManager.Instance.CloseWindow();
            if (popup != null)
            {
                popupPools[PopupType.Ok].Set(popup.GetComponent<IPopup>());
            }
        }
    }
}
