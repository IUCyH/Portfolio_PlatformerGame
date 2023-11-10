using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    ObjectPool<IItem> itemPool;
    List<IItem> tempItemList = new List<IItem>();
    [SerializeField]
    int randomSelectItemLimit;

    protected override void OnStart()
    {
        var items = Resources.LoadAll<GameObject>("Prefabs/Item");
        /*itemPool = new ObjectPool<IItem>(2, () =>
        {
            s
        });*/
    }

    /*public IItem CreateItem()
    {
        
    }

    void CleanList()
    {
        
    }*/
}
