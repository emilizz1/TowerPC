using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevelUpSpell : Spell
{

    [SerializeField] ParticleSystem particles;
    [SerializeField] int levelsToLevelUp;
    [SerializeField] GameObject rangeSprt;

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
            for (int i = 0; i < levelsToLevelUp; i++)
            {
                if (!myTower.MaxLevel())
                {
                    myTower.experience = myTower.experienceNeeded[myTower.currentLevel];
                    myTower.LevelUp();
                }
            }
        }
        rangeSprt.SetActive(false);
        particles.Stop();
    }
}
