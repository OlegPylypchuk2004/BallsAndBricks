using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T _prefab;
    private List<T> _pool;

    public ObjectPool(T prefab, int initialSize)
    {
        _prefab = prefab;
        _pool = new List<T>();

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private T CreateNewObject()
    {
        T newObject = GameObject.Instantiate(_prefab);
        newObject.gameObject.SetActive(false);
        _pool.Add(newObject);

        return newObject;
    }

    public T GetObject()
    {
        foreach (T pooledObject in _pool)
        {
            if (!pooledObject.gameObject.activeSelf)
            {
                pooledObject.gameObject.SetActive(true);
                return pooledObject;
            }
        }

        T newObject = CreateNewObject();
        newObject.gameObject.SetActive(true);

        return newObject;
    }

    public void ReturnObject(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
    }
}