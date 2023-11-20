using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    IItem item;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    
    public Sprite ItemImage { get; set; }
    
    public void Use()
    {
        item.Use();
    }

    public void Destroy()
    {
        item.Destroy();
    }

    public void Init(Vector3 pos, IItem item)
    {
        this.item = item;
        spriteRenderer.sprite = item.ItemImage;
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
