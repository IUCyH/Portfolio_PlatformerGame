using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string name;
    public uint level;
    public float hp;
    public float attackDamage;

    public void SetData(DataSnapshot dataSnapshot)
    {
        name = (string)dataSnapshot.Child("name").Value;
        level = (uint)dataSnapshot.Child("level").Value;
        hp = (float)dataSnapshot.Child("hp").Value;
        attackDamage = (float)dataSnapshot.Child("attackDamage").Value;
    }
}
