using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int initialSize;
    public bool oncePerWave;

    private Queue<GameObject> instances = new Queue<GameObject>();
    private Queue<GameObject> currentInstances = new Queue<GameObject>();

    private void Awake()
    {
        Assert.IsNotNull(prefab);
    }

    /// <summary>
    /// Initializes the object pool.
    /// </summary>
    public void Initialize()
    {
        for (var i = 0; i < initialSize; i++)
        {
            var obj = CreateInstance();
            obj.SetActive(false);
            instances.Enqueue(obj);
        }
    }

    /// <summary>
    /// Returns a new object from the pool.
    /// </summary>
    /// <returns>A new object from the pool.</returns>
    public GameObject GetObject()
    {
        var obj = instances.Count > 0 ? instances.Dequeue() : CreateInstance();
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Returns the specified game object to the pool where it came from.
    /// </summary>
    /// <param name="obj">The object to return to its origin pool.</param>
    public void ReturnObject(GameObject obj)
    {
        if (obj != null)
        {
            var pooledObject = obj.GetComponent<PooledObject>();
            obj.SetActive(false);
            obj.transform.SetParent(transform);

            if (oncePerWave)
            {
                if (!currentInstances.Contains(obj) && pooledObject.pool == this)
                {
                    currentInstances.Enqueue(obj);
                }
            }
            else
            {
                if (!instances.Contains(obj) && pooledObject.pool == this)
                {
                    instances.Enqueue(obj);
                }
            }
        }
    }

    /// <summary>
    /// Creates a new instance of the pooled object type.
    /// </summary>
    /// <returns>A new instance of the pooled object type.</returns>
    private GameObject CreateInstance()
    {
        var obj = Instantiate(prefab, transform);
        var pooledObject = obj.AddComponent<PooledObject>();
        pooledObject.pool = this;
        //obj.transform.SetParent(transform);
        return obj;
    }

    public void NewWave()
    {
        if (oncePerWave)
        {
            foreach(GameObject obj in currentInstances)
            {
                instances.Enqueue(obj);
            }
            currentInstances = new Queue<GameObject>();
        }
    }
}

/// <summary>
/// Utility class to identify the pool of a pooled object.
/// </summary>
public class PooledObject : MonoBehaviour
{
    public ObjectPool pool;
}
