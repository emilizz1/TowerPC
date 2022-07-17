using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSpell : Spell
{
    [SerializeField] float dealDamageTimer;
    [SerializeField] float damage;

    List<Enemy> enemies = new List<Enemy>();

    float timePassed;

    public override void Start()
    {
        base.Start();
        enemies = new List<Enemy>();
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed > dealDamageTimer)
        {
            timePassed -= dealDamageTimer;
            DealDamage();

        }
    }

    void DealDamage()
    {
        foreach(Enemy enemy in enemies)
        {
            if(enemy != null)
            {
                enemy.DealDamage(damage);
            }
        }
    }

    public override void DoEffect(GameObject enemy)
    {
        base.DoEffect(enemy);
        enemies.Add(enemy.GetComponent<Enemy>());
    }

    public override void StopEffect(GameObject enemy)
    {
        base.StopEffect(enemy);
        enemies.Remove(enemy.GetComponent<Enemy>());
    }
}
