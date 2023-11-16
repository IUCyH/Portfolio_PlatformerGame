using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterManager : Singleton<MonsterManager>
{
    const int MaxTryGetPositionCount = 500;
    
    [SerializeField]
    GameObject monsterPrefab;
    ObjectPool<MonsterController> monsterPool;
    
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
    }

    public void DestroyMonster(MonsterController monster)
    {
        monsterPool.Set(monster);
        monster.BelongedField.CreatedMonsters.Remove(monster);
        monster.BelongedField = null;
    }

    public void CreateMonsters(MonsterField monsterField, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var spawnPos = GetRandomPosition(monsterField);
            var monster = monsterPool.Get();

            monster.BelongedField = monsterField;
            monster.SetMonsterSpawnPos(spawnPos);
            monster.gameObject.SetActive(true);

            if (!monsterField.CreatedMonsters.Contains(monster))
            {
                monsterField.CreatedMonsters.Add(monster);
            }
        }
    }

    Vector3 GetRandomPosition(MonsterField monsterField)
    {
        Vector3 newPos;
        int tryCount = 0;
        float x, y;

        while (true)
        {
            x = Random.Range(monsterField.MaxSpawnPositionOnLeft.x, monsterField.MaxSpawnPositionOnRight.x);
            y = Random.Range(monsterField.MaxSpawnPositionOnLeft.y, monsterField.MaxSpawnPositionOnRight.y);

            newPos = new Vector3(x, y, 0f);
            tryCount++;
            
            if (tryCount > MaxTryGetPositionCount || CanSpawnInThisPosition(monsterField, newPos)) //최악의 경우 무한하게 위치를 얻어올 경우를 방지하기 위해 일정횟수 이상이 되면 함수 실행 강제종료
            {
                break;
            }
        }
        
        return newPos;
    }

    bool CanSpawnInThisPosition(MonsterField monsterField, Vector3 position)
    {
        for (int i = 0; i < monsterField.CreatedMonsters.Count; i++)
        {
            var monster = monsterField.CreatedMonsters[i];
            var distanceBetweenNewAndOther = (monster.transform.position - position).magnitude;

            if (distanceBetweenNewAndOther < minDistanceBetweenTwoMonsters)
            {
                return false;
            }
        }

        return true;
    }
}
