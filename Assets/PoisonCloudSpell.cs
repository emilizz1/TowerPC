using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloudSpell : Spell
{

    [SerializeField] float dealpoisonTimer;
    [SerializeField] ParticleSystem particles;

    List<Enemy> enemies = new List<Enemy>();

    public override void Start()
    {
        base.Start();
        enemies = new List<Enemy>();
        timePassed -= 0.1f;
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > dealpoisonTimer)
        {
            timePassed -= dealpoisonTimer;
            DealPoison();

        }
    }

    void DealPoison()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.isActiveAndEnabled)
            {
                if (enemy.GetComponent<PoisonDebuff>())
                {
                    enemy.GetComponent<PoisonDebuff>().AddStack();
                }
                else
                {
                    enemy.gameObject.AddComponent<PoisonDebuff>();
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

        StartCoroutine(StoppingAnimation());
    }

    IEnumerator StoppingAnimation()
    {
        particles.Stop();
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
