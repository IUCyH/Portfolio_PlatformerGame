using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    Sprite ItemImage { get; set; }
    
    void Use();
    void Destroy();
}
