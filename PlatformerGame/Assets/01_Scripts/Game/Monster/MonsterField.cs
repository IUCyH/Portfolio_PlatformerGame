using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterField : MonoBehaviour
{
    const int MonsterCreateCount = 5;
    const float MonsterCreateTiming = 1f;
    
    [SerializeField]
    Vector3 maxSpawnPosOnLeft;
    [SerializeField]
    Vector3 maxSpawnPosOnRight;

    public List<MonsterController> CreatedMonsters { get; private set; }
    public Vector3 MaxSpawnPositionOnLeft => maxSpawnPosOnLeft;
    public Vector3 MaxSpawnPositionOnRight => maxSpawnPosOnRight;

    void Start()
    {
        CreatedMonsters = new List<MonsterController>();
        StartCoroutine(Coroutine_Update());
        MonsterManager.Instance.CreateMonsters(this, MonsterCreateCount);
    }

    IEnumerator Coroutine_Update()
    {
        float timer = 0f;
        
        while (true)
        {
            if (CreatedMonsters.Count < 1)
            {
                timer += Time.deltaTime;
                if (timer > MonsterCreateTiming)
                {
                    MonsterManager.Instance.CreateMonsters(this, MonsterCreateCount);
                    timer = 0f;
                }
            }
            
            for (int i = 0; i < CreatedMonsters.Count; i++)
            {
                CreatedMonsters[i].Move();
                CreatedMonsters[i].Attack();
            }

            yield return null;
        }
    }
}
