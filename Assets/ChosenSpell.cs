using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChosenSpell : Spell
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] GameObject rangeSprt;
    [SerializeField] TowerStats statsToAdd;
    [SerializeField] int additionalGoldPerKill;

    Tower myTower;

    public override void SpellMoved()
    {
        base.SpellMoved();
        myTower = null;
        particles.Stop();
        Tile myTile = TileManager.instance.GetClosestTile(transform.position);
        Spot mySpot = myTile.GetClosestSpot(transform.position);

        if (mySpot.objBuilt)
        {
            myTower = mySpot.spotObj.GetComponent<Tower>();
            if (myTower != null)
            {
                particles.Play();
                return;
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
        if (myTower != null)
        {
            myTower.additionalGoldPerKill += additionalGoldPerKill;
            myTower.statsMultiplayers.CombineStats(statsToAdd);
        }
        rangeSprt.SetActive(false);
        particles.Stop();
    }

    public override void StopSpell()
    {
        myTower.additionalGoldPerKill -= additionalGoldPerKill;
        myTower.statsMultiplayers.RemoveStats(statsToAdd);
        base.StopSpell();
    }
}
