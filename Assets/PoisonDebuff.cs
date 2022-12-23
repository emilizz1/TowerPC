using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : Debuff
{
    const string DEBUFF_NAME = "Poison";

    const float DAMAGE_TIMER = 0.5f;

    List<float> damage;
    float timer;

    internal override void Start()
    {
        info = DebuffController.instance.GetInfo(DEBUFF_NAME);
        base.Start();
        timer = DAMAGE_TIMER;
        damage = new List<float>();
        damage.Add(info.effectAmount);
        damage.Add(info.effectAmount);
        damage.Add(info.effectAmount);
    }

    internal override void Update()
    {
        base.Update();
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = DAMAGE_TIMER;
            myEnemy.DealDamage(damage);
        }
    }

    internal override void ApplyDebuff()
    {
        myEnemy.debuffIcons.AddNewIcon(info.icon);
    }

    internal override void RemoveDebuff()
    {
        myEnemy.debuffIcons.RemoveIcon(info.icon);
    }
}
