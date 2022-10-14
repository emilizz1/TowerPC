using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    internal DebuffController.DebuffInfo info;

    internal Enemy myEnemy;
    int currentDebuffs;

    internal virtual void Start()
    {
        myEnemy = GetComponent<Enemy>();
        AddStack();
    }

    public void AddStack()
    {
        if (currentDebuffs >= info.maxDebuffs)
        {
            return;
        }

        currentDebuffs++;
        StartCoroutine(DoEffect());
    }

    IEnumerator DoEffect()
    {
        ApplyDebuff();
        yield return new WaitForSeconds(info.debuffTimer);
        if (myEnemy != null)
        {
            currentDebuffs--;
            RemoveDebuff();
            if (currentDebuffs <= 0)
            {
                Destroy(this);
            }
        }
    }

    internal virtual void ApplyDebuff()
    {

    }
    internal virtual void RemoveDebuff()
    {

    }
}
