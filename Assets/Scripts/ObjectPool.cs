using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T : MonoBehaviour
{
    public T prefab { get; }
    public bool autoExpand { get; set; }
    public Transform container { get; }

    public List<T> pool;

    public ObjectPool(T prefab, int count, Transform container, bool autoExpand)
    {
        this.prefab = prefab;
        this.container = container;
        this.autoExpand = autoExpand;

        this.CreatePool(count);
    }

    private void CreatePool(int count)
    {
        pool = new List<T>();

        for (int i = 0; i < count; i++)
            CreateObject();
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = Object.Instantiate(prefab, container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        pool.Add(createdObject);
        return createdObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var mono in pool)
            if (!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
            return element;

        if (autoExpand)
            return CreateObject(true);

        throw new Exception($"There is no free elements in pool of type {typeof(T)}");
    }
}
