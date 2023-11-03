    using System;
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    Transform slotsParent;
    [SerializeField]
    GameObject slotPrefab;
    ObjectPool<GameObject> slotPool;
    GridLayout gridLayout;
    
    void Start()
    {
        gridLayout = new GridLayout((-222, 180f), (150, 150f), 4);
        slotPool = new ObjectPool<GameObject>(4, () =>
        {
            var obj = Instantiate(slotPrefab);
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.SetParent(slotsParent);
            rectTransform.position = Vector3.zero;
            rectTransform.localScale = Vector3.one;

            obj.SetActive(false);

            return obj;
        });

        for (int i = 0; i < 8; i++)
        {
            var slot = slotPool.Get();
            gridLayout.AddItem(slot);
        }
        gridLayout.UpdateGrid();
    }
}
