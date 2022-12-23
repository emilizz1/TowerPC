using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevelUpSpell : Spell
{

    [SerializeField] ParticleSystem particles;

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
            myTower.experience = myTower.experienceNeeded[myTower.currentLevel];
            myTower.LevelUp();
        }
        Destroy(gameObject);
    }
}
