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
    ObjectPool<IPopup> popupPool;

    protected override void OnStart()
    {
        
    }
}
