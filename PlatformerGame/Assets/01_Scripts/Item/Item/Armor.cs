using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour, IItem
{
    public void Use()
    {
        
    }

    public void Destroy()
    {
        
    }

    public void Init(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
