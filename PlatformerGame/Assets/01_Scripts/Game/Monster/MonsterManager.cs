using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterManager : Singleton<MonsterManager>
{
    const int MaxGetPositionTryCount = 50;
    
    [SerializeField]
    GameObject monsterPrefab;
    ObjectPool<MonsterController> monsterPool;
    List<MonsterController> createdMonsters = new List<MonsterController>();
    
    [SerializeField]
    Vector3 maxSpawnPosOnRight;
    [SerializeField]
    Vector3 maxSpawnPosOnLeft;
    [SerializeField]
    float minDistanceBetweenTwoMonsters;

    protected override void OnStart()
    {
        monsterPool = new ObjectPool<MonsterController>(5, () =>
        {
            var obj = Instantiate(monsterPrefab);
            var monster = obj.GetComponent<MonsterController>();

            monster.InitMonster(transform);
            monster.gameObject.SetActive(false);
            return monster;
        });
        
        CreateMonsters(3);
    }

    void Update()
    {
        for (int i = 0; i < createdMonsters.Count; i++)
        {
            createdMonsters[i].Move();
        }
    }

    void CreateMonsters(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var spawnPos = GetRandomPosition();
            var monster = monsterPool.Get();
            
            monster.SetMonsterSpawnPos(spawnPos);
            monster.gameObject.SetActive(true);
            
            createdMonsters.Add(monster);
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 newPos;
        int tryCount = 0;
        float x, y, z;

        while (true)
        {
            x = Random.Range(maxSpawnPosOnLeft.x, maxSpawnPosOnRight.x);
            y = Random.Range(maxSpawnPosOnLeft.y, maxSpawnPosOnRight.y);
            z = Random.Range(maxSpawnPosOnLeft.z, maxSpawnPosOnRight.z);

            newPos = new Vector3(x, y, z);
            tryCount++;
            
            if (CanSpawnInThisPosition(newPos) || tryCount > MaxGetPositionTryCount) //최악의 경우 무한하게 위치를 얻어올 경우를 방지하기 위해 일정횟수 이상이 되면 함수 실행 강제종료
            {
                break;
            }
        }
        
        return newPos;
    }

    bool CanSpawnInThisPosition(Vector3 position)
    {
        for (int i = 0; i < createdMonsters.Count; i++)
        {
            var monster = createdMonsters[i];
            var distanceBetweenNewAndOther = (monster.transform.position - position).magnitude;

            if (distanceBetweenNewAndOther < minDistanceBetweenTwoMonsters)
            {
                return false;
            }
        }

        return true;
    }
}
