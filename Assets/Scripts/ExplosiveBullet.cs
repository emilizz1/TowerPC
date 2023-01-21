using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExplosiveBullet : Bullet
{
    [SerializeField] float explosionRadius;


    internal override void TargetReached()
    {
        if (target == null)
        {
            return;
        }
        Enemy myEnemy = target.GetComponent<Enemy>();
        List<Enemy> currentlyAliveEnemies = new List<Enemy>(EnemyManager.instance.aliveEnemies);

        List<float> halfDamage = new List<float>();
        for (int i = 0; i < damage.Count; i++)
        {
            halfDamage.Add(damage[i] / 2f);
        }
        foreach (Enemy enemy in currentlyAliveEnemies)
        {
            if (enemy == null)
            {
                continue;
            }
            if (enemy != myEnemy)
            {
                if (Vector3.Distance(target.transform.position, enemy.transform.position) < explosionRadius)
                {
                    enemy.GetComponent<Enemy>().DealDamage(halfDamage, damageColor);
                }
            }
        }

        base.TargetReached();
    }
}
