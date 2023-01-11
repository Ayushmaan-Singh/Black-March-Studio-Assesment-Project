using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    //14 is the max number of 
    private Queue<T> objectQueue;
    public int maxPoolSize;

    public ObjectPool(int maxPoolSize)
    {
        objectQueue = new Queue<T>();
        this.maxPoolSize = maxPoolSize;
    }

    public bool CanAddItemToPool()
    {
        return objectQueue.Count < maxPoolSize;
    }

    public void AddItemToPool(T obj)
    {
        if (CanAddItemToPool())
        {
            objectQueue.Enqueue(obj);
        }
        else
        {
            Debug.Log("Pooling Utility : Amount Exceeded Cannot Add More Item");
        }
    }

    public T GetItemFromPool()
    {
        if (objectQueue.Count > 0)
            return objectQueue.Dequeue();
        else
            return default;
    }
}
