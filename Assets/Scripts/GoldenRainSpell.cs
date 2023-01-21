using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenRainSpell : Spell
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
            AddDebuff();
        }
    }

    void AddDebuff()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.isActiveAndEnabled)
            {
                if (enemy.GetComponent<ExtraGoldDebuff>())
                {
                    enemy.GetComponent<ExtraGoldDebuff>().AddStack();
                }
                else
                {
                    enemy.gameObject.AddComponent<ExtraGoldDebuff>();
                }
            }
            else
            {
                enemiesToRemove.Add(enemy);
            }
        }

        foreach(Enemy removedEnemy in enemiesToRemove)
        {
            enemies.Remove(removedEnemy);
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
        duration--;
        if (durationNumber != null)
        {
            durationNumber.text = duration > 1 ? duration.ToString() : "";
        }
        if (duration > 0)
        {
            return;
        }
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
