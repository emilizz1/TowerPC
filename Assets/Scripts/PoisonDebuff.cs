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
        if (GlobalConditionHolder.poisonDisabled)
        {
            return;
        }
        base.Update();
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = DAMAGE_TIMER;
            if (GlobalConditionHolder.doublePoisonDamage)
            {
                List<float> doubledDamage = new List<float>();
                doubledDamage.Add(damage[0] * 2f);
                doubledDamage.Add(damage[1] * 2f);
                doubledDamage.Add(damage[2] * 2f);
                myEnemy.DealDamage(doubledDamage, Color.green, GlobalConditionHolder.poisonKills ? myEnemy.moneyOnKill : 0);
                return;
            }
            myEnemy.DealDamage(damage, Color.green, GlobalConditionHolder.poisonKills? myEnemy.moneyOnKill : 0);
        }
    }

    internal override void ApplyDebuff()
    {
        if (GlobalConditionHolder.poisonDisabled)
        {
            return;
        }
        myEnemy.debuffIcons.AddNewIcon(info.icon);
    }

    internal override void RemoveDebuff()
    {
        myEnemy.debuffIcons.RemoveIcon(info.icon);
    }
}
