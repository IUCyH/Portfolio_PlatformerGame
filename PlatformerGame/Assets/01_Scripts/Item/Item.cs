using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    IItem item;

    public void AddItem(IItem item)
    {
        this.item = item;
    }
    
    public void Use()
    {
        item.Use();
    }

    public void Destroy()
    {
        item.Destroy();
    }

    public void Init(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
