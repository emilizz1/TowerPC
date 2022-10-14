using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowstormSpell : Spell
{
    [SerializeField] float effectTimer;

    List<Enemy> enemies = new List<Enemy>();

    float timePassed;


    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > effectTimer)
        {
            timePassed -= effectTimer;
            SlowEnemies();

        }
    }

    void SlowEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<SlowDebuff>())
                {
                    enemy.GetComponent<SlowDebuff>().AddStack();
                }
                else
                {
                    enemy.gameObject.AddComponent<SlowDebuff>();
                }
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
