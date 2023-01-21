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
        enemyHealth = 0,
        enemyArmor = 1,
        enemyShield = 2,
        enemyHound = 3,

        enemyHealthH = 25,
        enemyArmorH = 26,
        enemyShieldH = 27,
        enemyHoundH = 28,

        enemyBoss = 50,
        enemyBossH = 75,

        basicBullet = 100,
        lightningBullet =101,
        arrow =102,
        spear = 103,
        explosion = 104,
        dart = 105

    }

    Dictionary<PoolNames, ObjectPool> poolDicktionary;

    protected override void Awake()
    {
        base.Awake();
        poolDicktionary = new Dictionary<PoolNames, ObjectPool>();
        foreach (NamedPool pool in allPools)
        {
            poolDicktionary.Add(pool.name, pool.pool);
            pool.pool.Initialize();
        }
    }

    public ObjectPool GetPool(PoolNames name)
    {
        return poolDicktionary[name];
    }

    public void NewTurn()
    {
        foreach(NamedPool pool in allPools)
        {
            pool.pool.NewWave();
        }
    }
}
