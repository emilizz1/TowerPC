using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPools : MonoSingleton<ObjectPools>
{
    [SerializeField] List<NamedPool> allPools;

    [Serializable]
    public  struct NamedPool
    {
        public ObjectPool pool;
        public PoolNames name;
    }

    public enum PoolNames
    {
        basicEnemy,
        basicBullet,
        lightningBullet,
        arrow,
        spear
    }

    Dictionary<PoolNames, ObjectPool> poolDicktionary;

    protected override void Awake()
    {
        base.Awake();
        poolDicktionary = new Dictionary<PoolNames, ObjectPool>();
        foreach (NamedPool pool in allPools)
        {
            poolDicktionary.Add(pool.name, pool.pool);
        }
    }

    public ObjectPool GetPool(PoolNames name)
    {
        return poolDicktionary[name];
    }
}
