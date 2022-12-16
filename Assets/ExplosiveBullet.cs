using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExplosiveBullet : Bullet
{
    [SerializeField] float explosionRadius;

    internal override void TargetReached()
    {
        Enemy myEnemy = target.GetComponent<Enemy>();
        foreach(Enemy enemy in EnemyManager.instance.aliveEnemies)
        {
            if(enemy != myEnemy)
            {           
                if (Vector3.Distance(transform.position, enemy.transform.position) < explosionRadius)
                {
                    target.GetComponent<Enemy>().DealDamage(damage);
                }
            }
        }

        base.TargetReached();
    }
}
