using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShards : Spell
{
    [SerializeField] float dealDamageTimer;
    [SerializeField] List<float> damage;
    [SerializeField] List<DamageTypes> damageType;


    List<Enemy> enemies = new List<Enemy>();

    void Start()
    {
        TowerStats damageBuff = PasiveTowerStatsController.GetStats(damageType);
        if (damageBuff.damage != null)
        {
            damage[0] += damage[0] * damageBuff.damage[0];
            damage[1] += damage[1] * damageBuff.damage[1];
            damage[2] += damage[2] * damageBuff.damage[2];
        }
        enemies = new List<Enemy>();
        timePassed -= 0.1f;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > dealDamageTimer)
        {
            timePassed -= dealDamageTimer;
            DealDamage();

        }
    }

    void DealDamage()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.isActiveAndEnabled)
            {
                enemy.DealDamage(damage, Color.blue);
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

        if (enemies.Count > 0)
        {
            audioSource.PlayOneShot(SoundsController.instance.GetAudioClip("Lightning2"));
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
    }
}
