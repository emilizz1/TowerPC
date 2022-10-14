using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpell : Spell
{
    List<Tower> towersAffected;

    public override void Activate()
    {
        base.Activate();
        towersAffected = GetAllTowersInRange();
        foreach (Tower tower in towersAffected)
        {
            DamageBuff damageBuff = tower.GetComponent<DamageBuff>();
            if (damageBuff == null)
            {
                tower.gameObject.AddComponent<DamageBuff>();
            }
            else
            {
                damageBuff.AddStack();
            }
        }
    }

    List<Tower> GetAllTowersInRange()
    {
        List<Tower> towers = new List<Tower>();
        foreach (Tower tower in TowerPlacer.allTowers)
        {
            if (Vector3.Distance(tower.transform.position, transform.position) < range)
            {
                towers.Add(tower);
            }
        }
        return towers;
    }

    public override void StopSpell()
    {
        base.StopSpell();
        foreach (Tower tower in towersAffected)
        {
            DamageBuff damageBuff = tower.GetComponent<DamageBuff>();
            if (damageBuff != null)
            {
                damageBuff.RemoveBuff();
            }
        }
    }
}
