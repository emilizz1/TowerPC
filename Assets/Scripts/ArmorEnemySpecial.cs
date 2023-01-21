using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorEnemySpecial : EnemySpecial
{
    [SerializeField] float additionalArmor;
    [SerializeField] float range;

    public override void UseSpecial()
    {
        Vector3 myPos = transform.position;

       foreach (Enemy enemy in EnemyManager.instance.aliveEnemies)
        {
            if(Vector3.Distance(myPos, enemy.transform.position) <= range)
            {
                enemy.currentHealth[1] += additionalArmor;
                enemy.tempMaxHealth[1] += additionalArmor;
                enemy.UpdateBars();
            }
        }

        GetComponent<EnemyMovement>().stopped = true;

        base.UseSpecial();
        particles.GetComponent<ParticleSystem>().Play();
    }

    internal override void SpecialFinished()
    {
        base.SpecialFinished();
        GetComponent<EnemyMovement>().stopped = false;
    }
}
