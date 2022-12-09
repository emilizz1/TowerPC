using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSpell : Spell
{
    [SerializeField] float dealDamageTimer;
    [SerializeField] List<float> damage;
    [SerializeField] List<PasiveTowerStatsController.DamageTypes> damageType;
    [SerializeField] List<ParticleSystem> particles;

    List<Enemy> enemies = new List<Enemy>();

    public override void Start()
    {
        base.Start();

        Tower.TowerStats damageBuff = PasiveTowerStatsController.GetStats(damageType);
        if (damageBuff.damage != null)
        {
            damage[0] += damage[0] * damageBuff.damage[0];
            damage[1] += damage[1] * damageBuff.damage[1];
            damage[2] += damage[2] * damageBuff.damage[2];
        }
        enemies = new List<Enemy>();
        timePassed -= 0.1f;
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

    public override void StopSpell()
    {
        //base.StopSpell();
        StartCoroutine(StopingAnimation());
    }

    IEnumerator StopingAnimation()
    {
        foreach(ParticleSystem particle in particles)
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
