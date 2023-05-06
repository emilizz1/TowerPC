using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTower : Tower
{
    Spot mySpot;
    List<Spot> adjacentSpots = new List<Spot>();

    bool adjacentDoubleBonusAdded;

    public override void Activate()
    {
        base.Activate();

        mySpot = GetComponentInParent<Spot>();
         adjacentSpots = mySpot.myTile.GetAdjacentSpots(mySpot, true);
        foreach (Tile tile in TileManager.instance.GetAdjacentTiles(mySpot.myTile.transform.position))
        {
            foreach (Spot spot in tile.GetAdjacentSpots(mySpot, true))
            {
                adjacentSpots.Add(spot);
            }
        }

        if (adjacentSpots.Count != 4)
        {
            TileManager.OnTilePlaced += NewTileAdded;
        }

        foreach (Spot spot in adjacentSpots)
        {
            foreach(TerrainBonus bonus in spot.terrainBonus)
            {
                if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(TowerType.Energy) == 1 && !adjacentDoubleBonusAdded)
                {
                    bonus.AddStats(this);
                    adjacentDoubleBonusAdded = true;
                }
                    bonus.AddStats(this);
            }
            spot.secondTerrainGetter = this;
        }
    }

    public void NewTileAdded(Tile newTile)
    {
        if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(TowerType.Energy) == 1 && !adjacentDoubleBonusAdded)
        {
            foreach (Spot spot in adjacentSpots)
            {
                foreach (TerrainBonus bonus in spot.terrainBonus)
                {
                    bonus.AddStats(this);
                    adjacentDoubleBonusAdded = true;
                }
            }
        }
        List<Spot> newAdjacentSpots = newTile.GetAdjacentSpots(mySpot, true);
        if (newAdjacentSpots.Count > 0)
        {
            adjacentSpots.Add(newAdjacentSpots[0]);
            newAdjacentSpots[0].secondTerrainGetter = this;
            if (adjacentSpots.Count == 4)
            {
                TileManager.OnTilePlaced -= NewTileAdded;
            }
        }
    }
}
