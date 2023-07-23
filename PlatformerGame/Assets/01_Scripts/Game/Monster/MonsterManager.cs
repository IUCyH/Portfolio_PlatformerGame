using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    [SerializeField]
    GameObject monsterPrefab;
    ObjectPool<MonsterController> monsterPool;

    protected override void OnStart()
    {
        monsterPool = new ObjectPool<MonsterController>(5, () =>
        {
            var obj = Instantiate(monsterPrefab);
            var monster = obj.GetComponent<MonsterController>();

            monster.Init(transform);
            return monster;
        });
    }
}
