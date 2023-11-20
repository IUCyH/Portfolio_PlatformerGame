using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string name;
    public uint level;
    public float levelUpProgress;
    public float hp;
    public float maxHP;
}

[Serializable]
public class ItemData
{
    public Sprite itemImage;
    public string name;
    public float attackDamage;
    public float defencePower;
    public float maxAttackDistance;
}
