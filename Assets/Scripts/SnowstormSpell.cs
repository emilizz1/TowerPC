using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowstormSpell : Spell
{
    [SerializeField] float effectTimer;
    [SerializeField] List<ParticleSystem> particles;

    List<Enemy> enemies = new List<Enemy>();

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
        List<Enemy> enemiesToRemove = new List<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.isActiveAndEnabled)
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
            else
            {
                enemiesToRemove.Add(enemy);

            }
        }

        foreach (Enemy enemy1 in enemiesToRemove)
        {
            enemies.Remove(enemy1);
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

    public override void Activate()
    {
        base.Activate();
        AchievementManager.StormPlaced();
    }

    public override void StopSpell()
    {
        //base.StopSpell();
        duration--;
        if (durationNumber != null)
        {
            durationNumber.text = duration > 1 ? duration.ToString() : "";
        }
        if (duration > 0)
        {
            return;
        }
        AchievementManager.StormRemoved();
        StartCoroutine(StopingAnimation());
    }

    IEnumerator StopingAnimation()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }

        Vector3 endPos = new Vector3(transform.position.x, transform.position.y + 100f, transform.position.z);
        float timePassed = 2f;
        while (timePassed > 0)
        {
            yield return null;
            timePassed -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * 20f);
        }
        Destroy(gameObject);
    }
}
