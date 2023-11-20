using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    ObjectPool<Item> itemPool;
    IItem[] itemScripts;
    [SerializeField]
    int randomSelectItemLimit;

    protected override void OnStart()
    {
        var itemDummy = Resources.Load<GameObject>("Prefabs/Item/ItemDummy");
        var itemPrefab = Resources.Load<GameObject>("Prefabs/Item/Item");
        itemPool = new ObjectPool<Item>(5, () =>
        {
            var obj = Instantiate(itemPrefab);
            obj.transform.SetParent(transform);
            obj.transform.position = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            
            obj.SetActive(false);

            return obj.GetComponent<Item>();
        });

        itemScripts = itemDummy.GetComponents<IItem>();
    }

    public void CreateItem(Vector3 initialPos)
    {
        var item = itemPool.Get();
        var itemScript = Random.Range(0, itemScripts.Length);
        
        item.Init(initialPos, itemScripts[itemScript]);
    }

    void CleanList()
    {
        
    }
}
