using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T : MonoBehaviour
{
    public T Prefab { get; }
    public bool AutoExpand { get; set; }
    public Transform Container { get; }

    public List<T> Pool;

    public ObjectPool(T prefab, int count, Transform container, bool autoExpand)
    {
        Prefab = prefab;
        Container = container;
        AutoExpand = autoExpand;

        this.CreatePool(count);
    }

    private void CreatePool(int count)
    {
        Pool = new List<T>();

        for (int i = 0; i < count; i++)
            CreateObject();
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = Object.Instantiate(Prefab, Container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        Pool.Add(createdObject);
        return createdObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var mono in Pool)
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

        if (AutoExpand)
            return CreateObject(true);

        throw new Exception($"There is no free elements in pool of type {typeof(T)}");
    }
}
