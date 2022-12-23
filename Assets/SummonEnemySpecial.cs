using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemySpecial : EnemySpecial
{
    [SerializeField] ObjectPools.PoolNames enemyPrefab;
    [SerializeField] int enemiesToSpawn;

    public override void UseSpecial()
    {
        base.UseSpecial();
        GetComponent<EnemyMovement>().stopped = true;
        StartCoroutine(SpawnEnemies());
        particles.GetComponent<ParticleSystem>().Play();
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Enemy newEnemy = EnemyManager.instance.SpawnEnemy(ObjectPools.instance.GetPool(enemyPrefab).GetObject(), transform.position);
            EnemyMovement myMovement = GetComponent<EnemyMovement>();
            newEnemy.movement.moveIndex = myMovement.moveIndex;
            newEnemy.movement.movementPath = myMovement.movementPath;
            yield return new WaitForSeconds(useTime/ enemiesToSpawn);
        }

    }

    internal override void SpecialFinished()
    {
        base.SpecialFinished();
        GetComponent<EnemyMovement>().stopped = false;
    }
}
