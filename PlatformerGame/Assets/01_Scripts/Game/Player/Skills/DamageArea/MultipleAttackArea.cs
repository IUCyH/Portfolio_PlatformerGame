using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleAttackArea : MonoBehaviour
{
    const string MonsterTag = "Monster";
    
    List<GameObject> monsters = new List<GameObject>();

    public List<GameObject> MonstersInsideOfArea => monsters;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(MonsterTag))
        {
            monsters.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(MonsterTag))
        {
            monsters.Remove(other.gameObject);
        }
    }
}
