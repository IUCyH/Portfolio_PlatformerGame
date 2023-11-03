using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : class
{
    Func<T> generateFunc;
    Queue<T> objQueue = new Queue<T>();
    int generateCount;

    public ObjectPool(int generateCount, Func<T> generateFunc)
    {
        this.generateCount = generateCount;
        this.generateFunc = generateFunc;

        Allocate();
    }

    void Allocate()
    {
        for (int i = 0; i < generateCount; i++)
        {
            objQueue.Enqueue(generateFunc());
        }
    }

    public T Get()
    {
        if (objQueue.Count < 1)
        {
            return generateFunc();
        }

        return objQueue.Dequeue();
    }

    public void Set(T obj)
    {
        objQueue.Enqueue(obj);
    }
}
