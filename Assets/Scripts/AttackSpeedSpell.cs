using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedSpell : Spell
{
    List<Tower> towersAffected;

    public override void Activate()
    {
        base.Activate();
        towersAffected = GetAllTowersInRange();
        foreach (Tower tower in towersAffected)
        {
            SpeedBuff speedBuff = tower.GetComponent<SpeedBuff>();
            if (speedBuff == null)
            {
                tower.gameObject.AddComponent<SpeedBuff>();
            }
            else
            {
                speedBuff.AddStack();
            }
        }
    }

    List<Tower> GetAllTowersInRange()
    {
        List<Tower> towers = new List<Tower>();
        foreach(Tower tower in TowerPlacer.allTowers)
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
            SpeedBuff speedBuff = tower.GetComponent<SpeedBuff>();
            if (speedBuff != null)
            {
                speedBuff.RemoveBuff();
            }
        }
    }
}
