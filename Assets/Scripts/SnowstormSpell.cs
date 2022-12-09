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

    public override void StopSpell()
    {
        //base.StopSpell();
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
