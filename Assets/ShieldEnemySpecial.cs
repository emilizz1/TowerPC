using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemySpecial : EnemySpecial
{
    [SerializeField] float time;
    [SerializeField] float range;

    List<Enemy> targeted;

    public override void UseSpecial()
    {
        Vector3 myPos = transform.position;
        targeted = new List<Enemy>();

        foreach (Enemy enemy in EnemyManager.instance.aliveEnemies)
        {
            if (Vector3.Distance(myPos, enemy.transform.position) <= range)
            {
                enemy.invincable = true;
                targeted.Add(enemy);
            }
        }

        GetComponent<EnemyMovement>().stopped = true;
        base.UseSpecial();
        particles.GetComponent<ParticleSystem>().Play();
    }

    internal override void SpecialFinished()
    {
        foreach(Enemy enemy in targeted)
        {
            enemy.invincable = false;
        }
        base.SpecialFinished();
        GetComponent<EnemyMovement>().stopped = false;
    }
}
