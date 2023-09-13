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
        var popups = Resources.LoadAll<GameObject>("Prefabs");
        var generateCount = popups.Length * 2;
        
        /*popupPool = new ObjectPool<IPopup>(generateCount, () =>
        {
            var popupPrefabs = popups;

            for (int i = 0; i < popupPrefabs.Length; i++)
            {
                var obj = Instantiate(popupPrefabs[i]);
                obj.transform.position = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                
                obj.SetActive(false);
            }
            
            
        });*/
    }
}
